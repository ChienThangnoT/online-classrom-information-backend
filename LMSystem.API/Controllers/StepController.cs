using LMSystem.Repository.Data;
using LMSystem.Services.Interfaces;
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
    }
}
