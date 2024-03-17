using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpGet("CourselistPagination")]
        public async Task<IActionResult> GetCourses([FromQuery] CourseFilterParameters filterParams)
        {
            var (Courses, CurrentPage, PageSize, TotalCourses, TotalPages) = await _courseService.GetCoursesWithFilters(filterParams);
            if (!Courses.Any())
            {
                return NotFound();
            }
            var response = new { Courses, CurrentPage, PageSize, TotalCourses, TotalPages };
            return Ok(response);
        }

        //[HttpGet("GetAllCourse")]
        //public async Task<IActionResult> GetAllCourse([FromQuery] PaginationParameter paginationParameter)
        //{
        //    try
        //    {
        //        var response = await _courseService.GetAllCourse(paginationParameter);
        //        var metadata = new
        //        {
        //            response.TotalCount,
        //            response.PageSize,
        //            response.CurrentPage,
        //            response.TotalPages,
        //            response.HasNext,
        //            response.HasPrevious
        //        };
        //        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        //        if (!response.Any())
        //        {
        //            return NotFound();
        //        }
        //        return Ok(response);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

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

        [HttpGet("GetCourseDetailById/{courseId}")]
        public async Task<IActionResult> GetCourseDetailByIdAsync(int courseId)
        {
            var courses = await _courseService.GetCourseDetailByIdAsync(courseId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("GetCourseDetailByCourseId-v2/{courseId}")]
        public async Task<IActionResult> GetCourseDetailByCourseIdAsync(int courseId)
        {
            var courses = await _courseService.GetCourseDetailByCourseIdAsync(courseId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpPost("AddCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourse(AddCourseModel addCourseModel)
        {
            var response = await _courseService.AddCourse(addCourseModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse(UpdateCourseModel updateCourseModel)
        {
            var response = await _courseService.UpdateCourse(updateCourseModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var response = await _courseService.DeleteCourse(courseId);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountTotalCourse")]
        public async Task<IActionResult> CountTotalCourse()
        {
            var response = await _courseService.CountTotalCourse();
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountTotalCourseUpToDate")]
        public async Task<IActionResult> CountTotalCourseUpToDate([FromQuery] DateTime to)
        {
            var response = await _courseService.CountTotalCourseUpToDate(to);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountTotalCourseByMonth")]
        public async Task<IActionResult> CountTotalCourseByMonth([FromQuery] int year)
        {
            var response = await _courseService.CountTotalCourseByMonth(year);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetYearList")]
        public async Task<IActionResult> GetYearList()
        {
            var response = await _courseService.GetYearList();
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountStudentPerCourse")]
        public async Task<IActionResult> CountStudentPerCourse()
        {
            var response = await _courseService.CountStudentPerCourse();
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
