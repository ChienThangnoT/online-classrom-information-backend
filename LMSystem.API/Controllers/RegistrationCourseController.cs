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
            if (string.IsNullOrEmpty(accountId) || courseId <= 0) 
            {
                return BadRequest("Account ID and Course ID must be provided.");
            }

            var isRegistered = await _registrationCourseService.CheckRegistrationCourse(accountId, courseId);
            if (isRegistered)
            {
                return Ok(new { IsRegistered = true });
            }
            else
            {
                return Ok(new { IsRegistered = false });
            }
        }

    }
}
