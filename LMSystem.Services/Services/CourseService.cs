﻿using LMSystem.Repository.Data;
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
        public async Task<IEnumerable<Course>> GetFilteredCourses(CourseFilterParameters filterParams)
        {
            return await _courseRepository.GetCoursesWithFilters(filterParams);
        }

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
    }
}