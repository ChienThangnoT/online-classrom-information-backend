using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<IEnumerable<Course>> GetFilteredCourses(CourseFilterParameters filterParams);
        public Task<IEnumerable<Course>> GetTopFavoriteCourses();

    }
}
