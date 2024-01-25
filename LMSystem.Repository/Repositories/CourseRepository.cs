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

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }
        public async Task<IEnumerable<Course>> GetCoursesWithFilters(CourseFilterParameters filterParams)
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

            // Apply pagination
            return await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetTopFavoriteCourses()
        {
            var topCourses = await _context.RegistrationCourses
             .GroupBy(rc => rc.CourseId)
             .OrderByDescending(g => g.Count())
             .Take(10)
             .Select(g => g.Key)
             .ToListAsync();

            var courses = await _context.Courses
                .Where(c => topCourses.Contains(c.CourseId))
                .ToListAsync();

            // If there are less than 10 courses, fill the gap with random courses
            if (courses.Count < 10)
            {
                var additionalCourses = await _context.Courses
                    .Where(c => !topCourses.Contains(c.CourseId))
                    .OrderBy(c => Guid.NewGuid())
                    .Take(10 - courses.Count)
                    .ToListAsync();

                courses.AddRange(additionalCourses);
            }

            return courses;
        }
    }
}
