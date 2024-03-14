using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepCompletedController : Controller
    {
        private readonly IStepCompletedService _stepCompletedService;
        public StepCompletedController(IStepCompletedService stepCompletedService)
        {
            _stepCompletedService = stepCompletedService;
        }
        [HttpPost("AddOrUpdateStepCompleted")]
        public async Task<IActionResult> AddOrUpdateStepCompleted(int registrationId, int stepId)
        {
            var response = await _stepCompletedService.AddOrUpdateStepCompleted(registrationId, stepId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet("GetStepIdByRegistrationId")]
        public async Task<IActionResult> GetStepIdByRegistrationId(int registrationId)
        {
            var response = await _stepCompletedService.GetStepIdByRegistrationId(registrationId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
