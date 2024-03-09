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
                    KnowdledgeDescription = addCourseModel.KnowdledgeDescription,
                    LinkCertificated = addCourseModel.LinkCertificated
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

                return new ResponeModel { Status = "Success", Message = "Added course successfully", DataObject =  course };
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

        public async Task<(IEnumerable<CourseListModel> Courses, int CurrentPage, int PageSize, int TotalCourses, int TotalPages)> GetCoursesWithFilters(CourseFilterParameters filterParams)
        {
            var query = _context.Courses.AsQueryable();

            // Apply filters
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

            int totalCourses = await query.CountAsync(); 
            int totalPages = (int)Math.Ceiling(totalCourses / (double)filterParams.PageSize);

            var courses = await query.Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                                     .Take(filterParams.PageSize)
                                     .Select(c => new CourseListModel
                                     {
                                         CourseId = c.CourseId,
                                         Title = c.Title,
                                         Price = c.Price,
                                         CourseCategory = string.Join(", ", c.CourseCategories.Select(cc => cc.Category.Name)),
                                         TotalDuration = c.TotalDuration,
                                         UpdateAt = c.UpdateAt,
                                         IsPublic = c.IsPublic
                                     })
                                     .ToListAsync();

            return (Courses: courses, CurrentPage: filterParams.PageNumber, PageSize: filterParams.PageSize, TotalCourses: totalCourses, TotalPages: totalPages);
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
                .Include(rc => rc.Registration)
                .ThenInclude(reg => reg.Course)
                .GroupBy(rc => rc.Registration.CourseId)
                .Select(group => new { CourseId = group.Key, AverageRating = group.Average(rc => rc.RatingStar) })
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

                var response = await _context.Courses
                    .Where(c => c.CourseId == updateCourseModel.CourseId)
                    .Select(c => new
                    {
                        c.CourseId,
                        c.Title,
                        c.Description,
                        c.ImageUrl,
                        c.VideoPreviewUrl,
                        c.Price,
                        c.SalesCampaign,
                        c.IsPublic,
                        c.CreateAt,
                        c.PublicAt,
                        c.UpdateAt,
                        c.TotalDuration,
                        c.CourseIsActive,
                        c.KnowdledgeDescription,
                        c.LinkCertificated,
                        CourseCategories = c.CourseCategories.Select(cc => new
                        {
                            cc.CategoryId,
                            Category = new
                            {
                                cc.Category.Name,
                                cc.Category.Description
                            }
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Update course successfully",
                    DataObject = response
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while update the course" };
            }
        }

        private Course submitCourseChange(Course course, UpdateCourseModel updateCourseModel)
        {
            if (!string.IsNullOrEmpty(updateCourseModel.Title))
            {
                course.Title = updateCourseModel.Title;
            }

            if (!string.IsNullOrEmpty(updateCourseModel.Description))
            {
                course.Description = updateCourseModel.Description;
            }

            if (!string.IsNullOrEmpty(updateCourseModel.ImageUrl))
            {
                course.ImageUrl = updateCourseModel.ImageUrl;
            }

            if (!string.IsNullOrEmpty(updateCourseModel.VideoPreviewUrl))
            {
                course.VideoPreviewUrl = updateCourseModel.VideoPreviewUrl;
            }

            if (updateCourseModel.Price.HasValue)
            {
                course.Price = updateCourseModel.Price.Value;
            }

            if (updateCourseModel.SalesCampaign.HasValue)
            {
                course.SalesCampaign = updateCourseModel.SalesCampaign.Value;
            }

            if (updateCourseModel.IsPublic.HasValue)
            {
                course.IsPublic = updateCourseModel.IsPublic.Value;
            }

            if (updateCourseModel.TotalDuration.HasValue)
            {
                course.TotalDuration = updateCourseModel.TotalDuration.Value;
            }

            if (updateCourseModel.CourseIsActive.HasValue)
            {
                course.CourseIsActive = updateCourseModel.CourseIsActive.Value;
            }

            if (!string.IsNullOrEmpty(updateCourseModel.KnowdledgeDescription))
            {
                course.KnowdledgeDescription = updateCourseModel.KnowdledgeDescription;
            }

            if (!string.IsNullOrEmpty(updateCourseModel.LinkCertificated))
            {
                course.LinkCertificated = updateCourseModel.LinkCertificated;
            }

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
