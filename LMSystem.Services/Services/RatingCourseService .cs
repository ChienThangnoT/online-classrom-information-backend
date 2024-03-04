using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class RatingCourseService : IRatingCourseService
    {
        private readonly IRatingCourseRepository _ratingCourseRepository;

        public RatingCourseService(IRatingCourseRepository ratingCourseRepository)
        {
            _ratingCourseRepository = ratingCourseRepository;
        }

        public async Task<RatingCourse> AddRatingAsync(RatingCourse ratingCourse)
        {
            return await _ratingCourseRepository.AddRatingAsync(ratingCourse);
        }

        public async Task<CourseRatingResult> GetCourseRating(int courseId)
        {
            return await _ratingCourseRepository.GetCourseRating(courseId);
        }

    }
}
