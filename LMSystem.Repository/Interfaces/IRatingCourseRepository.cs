using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IRatingCourseRepository
    {
        public Task<RatingCourse> AddRatingAsync(RatingCourse ratingCourse);
        public Task<double> GetCourseRating(int courseId);

    }
}
