using LMSystem.Repository.Data;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepController : ControllerBase
    {
        private readonly IStepService _stepService;

        public StepController(IStepService stepRepository)
        {
            _stepService = stepRepository;
        }

        [HttpPost("AddStep")]
        public async Task<IActionResult> AddStep(AddStepModel addStepModel)
        {
            var response = await _stepService.AddStep(addStepModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        //[HttpPut("UpdateStep")]
        //public async Task<IActionResult> UpdateStep(UpdateStepModel updateStepModel)
        //{
        //    var response = await _stepService.UpdateStep(updateStepModel);
        //    if (response.Status == "Error")
        //    {
        //        return Conflict(response);
        //    }

        //    return Ok(response);
        //}

        [HttpGet("LearningProgress")]
        public async Task<IActionResult> GetCourseProgress(int registrationId)
        {
            var progress = await _stepService.CheckCourseProgress(registrationId);
            if (progress == null)
            {
                return NotFound("Learning progress not found for the given registration ID.");
            }
            return Ok(progress);

        }
    }
}
