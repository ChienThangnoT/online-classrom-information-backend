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
    }
}
