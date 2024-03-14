using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async Task<(IEnumerable<CourseListModel> Courses, int CurrentPage, int PageSize, int TotalCourses, int TotalPages)> GetCoursesWithFilters(CourseFilterParameters filterParams)
        {
            return await _courseRepository.GetCoursesWithFilters(filterParams);
        }

        //public async Task<PagedList<CourseListModel>> GetAllCourse(PaginationParameter paginationParameter)
        //{
        //    return await _courseRepository.GetAllCourse(paginationParameter);
        //}

        public async Task<IEnumerable<Course>> GetTopCoursesByStudentJoined(int numberOfCourses)
        {
            return await _courseRepository.GetTopCoursesByStudentJoined(numberOfCourses);
        }

        public async Task<IEnumerable<Course>> GetTopCoursesByRating(int numberOfCourses)
        {
            return await _courseRepository.GetTopCoursesByRating(numberOfCourses);
        }
        public async Task<IEnumerable<Course>> GetTopCoursesBySales(int numberOfCourses)
        {
            return await _courseRepository.GetTopCoursesBySales(numberOfCourses);
        }

        public async Task<Course> GetCourseDetailByIdAsync(int courseId)
        {
            return await _courseRepository.GetCourseDetailByIdAsync(courseId);
        } 
        
        public async Task<CourseListModel> GetCourseDetailByCourseIdAsync(int courseId)
        {
            return await _courseRepository.GetCourseDetailByCourseIdAsync(courseId);
        }

        public async Task<ResponeModel> AddCourse(AddCourseModel addCourseModel)
        {
            return await _courseRepository.AddCourse(addCourseModel);
        }
        public async Task<ResponeModel> DeleteCourse(int courseId)
        {
            return await _courseRepository.DeleteCourse(courseId);
        }

        public async Task<ResponeModel> UpdateCourse(UpdateCourseModel updateCourseModel)
        {
            return await _courseRepository.UpdateCourse(updateCourseModel);
        }

        public async Task<ResponeModel> CountTotalCourse()
        {
            return await _courseRepository.CountTotalCourse();
        }

        public async Task<ResponeModel> CountTotalCourseUpToDate(DateTime to)
        {
            return await _courseRepository.CountTotalCourseUpToDate(to);
        }

        public async Task<ResponeModel> CountTotalCourseByMonth(int year)
        {
            return await _courseRepository.CountTotalCourseByMonth(year);
        }

        public async Task<ResponeModel> GetYearList()
        {
            return await _courseRepository.GetYearList();
        }

        public async Task<ResponeModel> CountStudentPerCourse()
        {
            return await _courseRepository.CountStudentPerCourse();
        }
    }
}
