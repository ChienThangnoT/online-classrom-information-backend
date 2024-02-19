using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class RatingCourseRepository : IRatingCourseRepository
    {
        // Assuming you have a DbContext or similar for data access
        private readonly LMOnlineSystemDbContext _context;

        public RatingCourseRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<RatingCourse> AddRatingAsync(RatingCourse ratingCourse)
        {
            _context.RatingCourses.Add(ratingCourse);
            await _context.SaveChangesAsync();
            return ratingCourse;
        }
    }
}
