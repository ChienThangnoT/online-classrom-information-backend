using LMSystem.Services.Interfaces;
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
