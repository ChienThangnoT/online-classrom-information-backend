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
            bool ratingExists = await _context.RatingCourses
                .AnyAsync(rc => rc.RegistrationId == ratingCourse.RegistrationId);

            if (ratingExists)
            {
                throw new InvalidOperationException("You have already rated this course.");
            }
            // First, find the registration associated with this rating
            var registration = await _context.RegistrationCourses
                .Include(rc => rc.StepCompleteds)
                .FirstOrDefaultAsync(rc => rc.RegistrationId == ratingCourse.RegistrationId);

            if (registration == null)
            {
                throw new ArgumentException("Invalid registration ID.");
            }

            // Check if all steps are completed and learning progress is 100%
            bool? allStepsCompleted = registration.IsCompleted; // Assuming existence of StepCompleted means step is completed
            double? learningProgressComplete = registration.LearningProgress;

            if (allStepsCompleted == false || learningProgressComplete < 1)
            {
                throw new InvalidOperationException("Cannot rate the course. All steps must be completed and learning progress must be 100%.");
            }

            // If checks pass, proceed to add the rating
            _context.RatingCourses.Add(ratingCourse);
            await _context.SaveChangesAsync();
            return ratingCourse;
        }
    }
}
