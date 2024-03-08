using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
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

                return new ResponeModel { Status = "Success", Message = "Added course successfully", DataObject = course };
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
            if (!string.IsNullOrEmpty(filterParams.Search))
            {
                query = query.Where(o => o.Title.Contains(filterParams.Search));
            }

            switch (filterParams.Sort)
            {
                case "title_asc":
                    query = query.OrderBy(p => p.Title);
                    break;
                case "title_desc":
                    query = query.OrderByDescending(p => p.Title);
                    break;
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
            }

            int totalCourses = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCourses / (double)filterParams.PageSize);

            var courses = await query.Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                                     .Take(filterParams.PageSize)
                                     .Select(c => new CourseListModel
                                     {
                                         CourseId = c.CourseId,
                                         Title = c.Title,
                                         ImageUrl = c.ImageUrl,
                                         Price = c.Price,
                                         CourseCategory = string.Join(", ", c.CourseCategories.Select(cc => cc.Category.Name)),
                                         TotalDuration = c.TotalDuration,
                                         UpdateAt = c.UpdateAt,
                                         CourseIsActive = c.CourseIsActive
                                     })
                                     .ToListAsync();

            return (Courses: courses, CurrentPage: filterParams.PageNumber, PageSize: filterParams.PageSize, TotalCourses: totalCourses, TotalPages: totalPages);
        }

        //public async Task<PagedList<CourseListModel>> GetAllCourse(PaginationParameter paginationParameter)
        //{
        //    if (_context == null)
        //    {
        //        return null;
        //    }

        //    var courseQuery = _context.Courses.AsQueryable();

        //    if (!string.IsNullOrEmpty(paginationParameter.Search))
        //    {
        //        courseQuery = courseQuery.Where(o => o.Title.Contains(paginationParameter.Search));
        //    }

        //    switch (paginationParameter.Sort)
        //    {
        //        case "title_asc":
        //            courseQuery = courseQuery.OrderBy(p => p.Title);
        //            break;
        //        case "title_desc":
        //            courseQuery = courseQuery.OrderByDescending(p => p.Title);
        //            break;
        //        case "price_asc":
        //            courseQuery = courseQuery.OrderBy(p => p.Price);
        //            break;
        //        case "price_desc":
        //            courseQuery = courseQuery.OrderByDescending(p => p.Price);
        //            break;
        //    }

        //    var totalItems = await courseQuery.CountAsync();

        //    var items = await courseQuery.Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
        //                     .Take(paginationParameter.PageSize)
        //                     .Select(course => new CourseListModel
        //                     {
        //                         CourseId = course.CourseId,
        //                         Title = course.Title,
        //                         ImageUrl = course.ImageUrl,
        //                         Price = course.Price,
        //                         CourseCategory = string.Join(", ", course.CourseCategories.Select(cc => cc.Category.Name)), 
        //                         TotalDuration = course.TotalDuration,
        //                         UpdateAt = course.UpdateAt,
        //                         CourseIsActive = course.CourseIsActive
        //                     })
        //                     .ToListAsync();


        //    var pagedList = new PagedList<CourseListModel>(items, totalItems, paginationParameter.PageNumber, paginationParameter.PageSize);

        //    return pagedList;
        //}

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
            course.LinkCertificated = updateCourseModel.LinkCertificated;
            course.UpdateAt = DateTime.UtcNow;

            var categoriesToRemove = course.CourseCategories
                .Where(cc => !updateCourseModel.CategoryList.Contains(cc.CategoryId))
                .ToList();

            foreach (var categoryToRemove in categoriesToRemove)
            {
                course.CourseCategories.Remove(categoryToRemove);
            }

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

                return new ResponeModel { Status = "Success", Message = "Course deleted successfully", DataObject = existingCourse };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while deleting the course" };
            }
        }

        public async Task<ResponeModel> CountTotalCourse()
        {
            try
            {
                var totalCourses = await _context.Courses.CountAsync();
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Total courses counted successfully",
                    DataObject = totalCourses
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while counting total courses in the system" };
            }
        }

        public async Task<ResponeModel> CountTotalCourseUpToDate(DateTime to)
        {
            try
            {
                var totalCourses = await _context.Courses
                    .Where(c => c.CreateAt <= to)
                    .CountAsync();
                return new ResponeModel
                {
                    Status = "Success",
                    Message = $"Total courses in the system up to {to} counted successfully",
                    DataObject = totalCourses
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while counting total courses in the system" };
            }
        }

        public async Task<ResponeModel> CountTotalCourseByMonth(int year)
        {
            try
            {
                var totalCoursesByMonth = await _context.Courses
                    .Where(c => c.CreateAt.Value.Year == year)
                    .GroupBy(c => c.CreateAt.Value.Month)
                    .Select(g => new { Month = g.Key, TotalCourses = g.Count() })
                    .OrderBy(g => g.Month)
                    .ToListAsync();

                int[] array = new int[12];

                foreach (var coursesByMonth in totalCoursesByMonth)
                {
                    array[coursesByMonth.Month - 1] = coursesByMonth.TotalCourses;
                }

                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var jsonData = JsonConvert.SerializeObject(array, jsonSerializerSettings);

                return new ResponeModel
                {
                    Status = "Success",
                    Message = $"Total courses created for each month in {year} retrieved successfully",
                    DataObject = jsonData
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while retrieving total courses by month"
                };
            }
        }

        public async Task<ResponeModel> GetYearList()
        {
            try
            {
                var distinctYears = await _context.Courses
                    .Select(c => c.CreateAt.Value.Year)
                    .Distinct()
                    .OrderBy(year => year)
                    .ToListAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Distinct years for courses retrieved successfully",
                    DataObject = distinctYears
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while retrieving distinct years for courses"
                };
            }
        }

        public async Task<ResponeModel> CountStudentPerCourse()
        {
            try
            {
                var studentsPerCourse = await _context.Courses
                    .Select(c => new StudentPerCourseModel
                    {
                        CourseId = c.CourseId,
                        CourseTitle = c.Title,
                        TotalStudents = c.RegistrationCourses.Count()
                    })
                    .ToListAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Total number of students per course retrieved successfully",
                    DataObject = studentsPerCourse
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while retrieving total number of students per course",
                };
            }
        }
    }
}
