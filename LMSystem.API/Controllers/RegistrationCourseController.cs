using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationCourseController : Controller
    {
        private readonly IRegistrationCourseService _registrationCourseService;

        public RegistrationCourseController(IRegistrationCourseService registrationCourseService)
        {
            _registrationCourseService = registrationCourseService;
        }
        [HttpGet("GetRegisterCourseListByAccountId")]
        [Authorize]
        public async Task<IActionResult> GetRegisterCourseListByAccountId(string accountId)
        {
            var courses = await _registrationCourseService.GetRegisterCourseListByAccountId(accountId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }
        [HttpGet("GetCompletedLearningCourseListByAccountId")]
        [Authorize]
        public async Task<IActionResult> GetCompletedLearningCourseListByAccountId(string accountId)
        {
            var courses = await _registrationCourseService.GetCompletedLearningCourseByAccountId(accountId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("CheckRegistration")]
        [Authorize]
        public async Task<IActionResult> CheckRegistrationCourse([FromQuery] string accountId, [FromQuery] int courseId)
        {
            var result = await _registrationCourseService.CheckRegistrationCourse(accountId, courseId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetRegisterCourseListByParentAccountId")]
        [Authorize]
        public async Task<IActionResult> GetRegisterCourseListByParentAccountId(string accountId)
        {
            var courses = await _registrationCourseService.GetRegisterCourseListByParentAccountId(accountId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("GetCompletedLearningCourseByParentAccountId")]
        [Authorize]
        public async Task<IActionResult> GetCompletedLearningCourseByParentAccountId(string accountId)
        {
            var courses = await _registrationCourseService.GetCompletedLearningCourseByParentAccountId(accountId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("GetUncompletedLearningCourseByParentAccountId")]
        [Authorize]
        public async Task<IActionResult> GetUncompletedLearningCourseByParentAccountId(string accountId)
        {
            var courses = await _registrationCourseService.GetUncompletedLearningCourseByParentAccountId(accountId);
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }
    }
}
