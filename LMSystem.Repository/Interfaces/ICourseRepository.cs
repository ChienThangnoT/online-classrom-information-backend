using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface ICourseRepository
    {
        public Task<IEnumerable<Course>> GetCoursesWithFilters(CourseFilterParameters filterParams);
        public Task<IEnumerable<Course>> GetTopCoursesByStudentJoined(int numberOfCourses);
        public Task<IEnumerable<Course>> GetTopCoursesBySales(int numberOfCourses);
        public Task<IEnumerable<Course>> GetTopCoursesByRating(int numberOfCourses);
        public Task<Course> GetCourseDetailByIdAsync(int courseId);
        //public Task<ResponeModel> AddCourse(AddCourseModel addCourseModel);
        //public Task<ResponeModel> UpdateCourse(UpdateCourseModel updateCourseModel);
    }
}
