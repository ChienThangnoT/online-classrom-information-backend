using LMSystem.Repository.Data;
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
        public async Task<IActionResult> GetTopFavoriteCourses(int numberOfCourses)
        {
            var courses = await _courseService.GetTopFavoriteCourses(numberOfCourses);
            return Ok(courses);
        }
    }
}
