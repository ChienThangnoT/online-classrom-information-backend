using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
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
        public Task<(IEnumerable<CourseListModel> Courses, int CurrentPage, int PageSize, int TotalCourses, int TotalPages)> GetCoursesWithFilters(CourseFilterParameters filterParams);
        //public Task<PagedList<CourseListModel>> GetAllCourse(PaginationParameter paginationParameter);

        public Task<IEnumerable<Course>> GetTopCoursesByStudentJoined(int numberOfCourses);
        public Task<IEnumerable<Course>> GetTopCoursesBySales(int numberOfCourses);

        public Task<IEnumerable<Course>> GetTopCoursesByRating(int numberOfCourses);

        public Task<Course> GetCourseDetailByIdAsync(int courseId);
        public Task<CourseListModel> GetCourseDetailByCourseIdAsync(int courseId);
        public Task<ResponeModel> AddCourse(AddCourseModel addCourseModel);
        public Task<ResponeModel> UpdateCourse(UpdateCourseModel updateCourseModel);
        public Task<ResponeModel> DeleteCourse(int courseId);
        public Task<ResponeModel> CountTotalCourse();
        public Task<ResponeModel> CountTotalCourseUpToDate(DateTime to);
        public Task<ResponeModel> CountTotalCourseByMonth(int year);
        public Task<ResponeModel> GetYearList();
        public Task<ResponeModel> CountStudentPerCourse();
    }
}
