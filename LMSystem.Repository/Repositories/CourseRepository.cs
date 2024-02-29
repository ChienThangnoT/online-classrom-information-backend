using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMSystem.Repository.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        // Assuming you have a DbContext or similar for data access
        private readonly LMOnlineSystemDbContext _context;

        public CourseRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<ResponeModel> AddCourse(AddCourseModel addCourseModel)
        {
            try
            {
                var course = new Course
                {
                    Title = addCourseModel.Title,
                    Description = addCourseModel.Description,
                    ImageUrl = addCourseModel.ImageUrl,
                    VideoPreviewUrl = addCourseModel.VideoPreviewUrl,
                    Price = addCourseModel.Price,
                    SalesCampaign = addCourseModel.SalesCampaign,
                    IsPublic = addCourseModel.IsPublic,
                    CreateAt = DateTime.UtcNow,
                    PublicAt = DateTime.UtcNow,
                    TotalDuration = addCourseModel.TotalDuration,
                    CourseIsActive = addCourseModel.CourseIsActive,
                    KnowdledgeDescription = addCourseModel.KnowdledgeDescription
                };

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                foreach (var categoryId in addCourseModel.CategoryList)
                {
                    var courseCategory = new CourseCategory
                    {
                        CourseId = course.CourseId,
                        CategoryId = categoryId
                    };

                    _context.CourseCategories.Add(courseCategory);
                }
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Added course successfully", DataObject = "CourseID: "+ course.CourseId };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the course" };
            }
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseDetailByIdAsync(int courseId)
        {
            var course = await _context.Courses
            .Include(c => c.Sections)
            .ThenInclude(s => s.Steps)
            .FirstOrDefaultAsync(c => c.CourseId == courseId);

            return course;
        }

        public async Task<IEnumerable<CourseListModel>> GetCoursesWithFilters(CourseFilterParameters filterParams)
        {
            var query = _context.Courses.AsQueryable();

            if (filterParams.CategoryIds != null && filterParams.CategoryIds.Any())
            {
                query = query.Where(c => c.CourseCategories.Any(cc => filterParams.CategoryIds.Contains(cc.CategoryId)));
            }

            if (filterParams.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= filterParams.MinPrice.Value);
            }

            if (filterParams.MaxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= filterParams.MaxPrice.Value);
            }

            var coursesWithFilter = await query
                                        .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                                        .Take(filterParams.PageSize)
                                        .Select(c => new CourseListModel
                                        {
                                            CourseId = c.CourseId,
                                            Title = c.Title,
                                            Price = c.Price,
                                            // Assuming CourseModel has a Categories property of type List<string> to hold category names
                                            CourseCategory = string.Join(", ", c.CourseCategories.Select(cc => cc.Category.Name)), // Joining category names
                                            TotalDuration = c.TotalDuration,
                                            UpdateAt = c.UpdateAt,
                                            IsPublic = c.IsPublic
                                        })
                                        .ToListAsync();

            return coursesWithFilter;
        }

        public async Task<IEnumerable<Course>> GetTopCoursesByStudentJoined(int numberOfCourses)
        {
            var topCourses = await _context.RegistrationCourses
                .GroupBy(rc => rc.CourseId)
                .OrderByDescending(g => g.Count())
                .Take(numberOfCourses)
                .Select(g => g.Key)
                .ToListAsync();

            var courses = await _context.Courses
                .Where(c => topCourses.Contains(c.CourseId))
                .ToListAsync();

            // Fill with random courses if needed
            if (courses.Count < numberOfCourses)
            {
                var additionalCourses = await _context.Courses
                    .Where(c => !topCourses.Contains(c.CourseId))
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numberOfCourses - courses.Count)
                    .ToListAsync();

                courses.AddRange(additionalCourses);
            }

            return courses;
        }

        public async Task<IEnumerable<Course>> GetTopCoursesBySales(int numberOfCourses)
        {
            var topCourses = await _context.Orders
                .GroupBy(o => o.CourseId)
                .OrderByDescending(g => g.Count())
                .Take(numberOfCourses)
                .Select(g => g.Key)
                .ToListAsync();

            var courses = await _context.Courses
                .Where(c => topCourses.Contains(c.CourseId))
                .ToListAsync();

            if (courses.Count < numberOfCourses)
            {
                var additionalCourses = await _context.Courses
                    .Where(c => !topCourses.Contains(c.CourseId))
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numberOfCourses - courses.Count)
                    .ToListAsync();

                courses.AddRange(additionalCourses);
            }

            return courses;
        }

        public async Task<IEnumerable<Course>> GetTopCoursesByRating(int numberOfCourses)
        {
            var topCourseIds = await _context.RatingCourses
                .Include(rc => rc.Registration) // Include Registration navigation property
                .ThenInclude(reg => reg.Course) // Then include Course navigation property from Registration
                .GroupBy(rc => rc.Registration.CourseId) // Group by CourseId from RegistrationCourse
                .Select(group => new { CourseId = group.Key, AverageRating = group.Average(rc => rc.RatingStar) }) // Assuming the property is named RatingValue
                .OrderByDescending(x => x.AverageRating)
                .Take(numberOfCourses)
                .Select(x => x.CourseId)
                .ToListAsync();

            var courses = await _context.Courses
                .Where(c => topCourseIds.Contains(c.CourseId))
                .ToListAsync();

            if (courses.Count < numberOfCourses)
            {
                var additionalCourses = await _context.Courses
                    .Where(c => !topCourseIds.Contains(c.CourseId))
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numberOfCourses - courses.Count)
                    .ToListAsync();

                courses.AddRange(additionalCourses);
            }

            return courses;
        }

        public async Task<ResponeModel> UpdateCourse(UpdateCourseModel updateCourseModel)
        {
            try
            {
                var existingCourse = await _context.Courses
                     .Include(c => c.CourseCategories)
                     .FirstOrDefaultAsync(c => c.CourseId == updateCourseModel.CourseId);
                if (existingCourse == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Course not found" };
                }
                existingCourse = submitCourseChange(existingCourse, updateCourseModel);

                //_context.Courses.Update(existingCourse);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Update course successfully", DataObject = existingCourse };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while update the course" };
            }
        }

        private Course submitCourseChange(Course course, UpdateCourseModel updateCourseModel)
        {          
            course.Title = updateCourseModel.Title;
            course.Description = updateCourseModel.Description;
            course.ImageUrl = updateCourseModel.ImageUrl;
            course.VideoPreviewUrl = updateCourseModel.VideoPreviewUrl;
            course.Price = updateCourseModel.Price;
            course.SalesCampaign = updateCourseModel.SalesCampaign;
            course.IsPublic = updateCourseModel.IsPublic;
            course.TotalDuration = updateCourseModel.TotalDuration;
            course.CourseIsActive = updateCourseModel.CourseIsActive;
            course.KnowdledgeDescription = updateCourseModel.KnowdledgeDescription;
            course.UpdateAt = DateTime.UtcNow;
            //remore old category
            var categoriesToRemove = course.CourseCategories
                .Where(cc => !updateCourseModel.CategoryList.Contains(cc.CategoryId))
                .ToList();

            foreach (var categoryToRemove in categoriesToRemove)
            {
                course.CourseCategories.Remove(categoryToRemove);
            }
            //add new category
            var categoriesToAdd = updateCourseModel.CategoryList
                .Where(categoryId => !course.CourseCategories.Any(cc => cc.CategoryId == categoryId))
                .Select(categoryId => new CourseCategory { CategoryId = categoryId })
                .ToList();

            foreach (var categoryToAdd in categoriesToAdd)
            {
                course.CourseCategories.Add(categoryToAdd);
            }

            return course;
        }

        public async Task<ResponeModel> DeleteCourse(int courseId)
        {
            try
            {
                var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

                if (existingCourse == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No course were found for the specified course id"
                    };
                }
                if (existingCourse.CourseIsActive == false)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "Course already inactive"
                    };
                }
                existingCourse.CourseIsActive = false;
                existingCourse.IsPublic = false;

                _context.Entry(existingCourse).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Course deleted successfully", DataObject = existingCourse};
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while deleting the course" };
            }
        }
    }
}
