﻿using LMSystem.Repository.Data;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseRepository)
        {
            _courseService = courseRepository;
        }

        [HttpGet("SelectCourselistPagination")]
        public async Task<IActionResult> GetCourses([FromQuery] CourseFilterParameters filterParams)
        {
            var courses = await _courseService.GetFilteredCourses(filterParams);
            if (courses == null)
            {
                return NotFound();
            }

            return Ok(courses);
        }

        [HttpGet("top-favorites")]
        public async Task<IActionResult> GetTopFavoriteCourses()
        {
            var courses = await _courseService.GetTopFavoriteCourses();
            return Ok(courses);
        }
        
        [HttpGet("GetCourseDetailById")]
        public async Task<IActionResult> GetCourseDetailById(int courseId)
        {
            var courses = await _courseService.GetCourseDetailByIdAsync(courseId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }

    }
}
