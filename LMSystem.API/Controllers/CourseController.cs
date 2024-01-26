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

        [HttpGet("TopFavoritesCourseBaseStudentJoined")]
        public async Task<IActionResult> GetTopCoursesByStudentJoined(int numberOfCourses)
        {
            var courses = await _courseService.GetTopCoursesByStudentJoined(numberOfCourses);
            return Ok(courses);
        }

        [HttpGet("TopFavoritesCourseBaseSales")]
        public async Task<IActionResult> GetTopCoursesBySales(int numberOfCourses)
        {
            var courses = await _courseService.GetTopCoursesBySales(numberOfCourses);
            return Ok(courses);
        }

        [HttpGet("TopFavoritesCourseBaseRating")]
        public async Task<IActionResult> GetTopCoursesByRating(int numberOfCourses)
        {
            var courses = await _courseService.GetTopCoursesByRating(numberOfCourses);
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

        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse(AddCourseModel addCourseModel)
        {
            var response = await _courseService.AddCourse(addCourseModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }
    }
}
