using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class CourseRepository: ICourseRepository
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
    }
}
