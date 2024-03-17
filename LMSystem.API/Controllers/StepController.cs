using LMSystem.Repository.Data;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStep(AddStepModel addStepModel)
        {
            var response = await _stepService.AddStep(addStepModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [HttpPut("UpdateStep")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStep(UpdateStepModel updateStepModel)
        {
            var response = await _stepService.UpdateStep(updateStepModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [HttpGet("LearningProgress")]
        [Authorize]
        public async Task<IActionResult> GetCourseProgress(int registrationId)
        {
            var progress = await _stepService.CheckCourseProgress(registrationId);
            if (progress == null)
            {
                return NotFound("Learning progress not found for the given registration ID.");
            }
            return Ok(progress);

        }
        [HttpGet("GetStepsBySectionId")]
        public async Task<IActionResult> GetStepsBySectionId(int sectionId)
        {
            var response = await _stepService.GetStepsBySectionId(sectionId);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteStep")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStep(int stepId)
        {
            var response = await _stepService.DeleteStep(stepId);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
