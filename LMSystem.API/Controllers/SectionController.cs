using LMSystem.Repository.Data;
using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionRepository)
        {
            _sectionService = sectionRepository;
        }

        [HttpPost("AddSection")]
        public async Task<IActionResult> AddSection(AddSectionModel addSectionModel)
        {
            var response = await _sectionService.AddSection(addSectionModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [HttpPut("UpdateSection")]
        public async Task<IActionResult> UpdateSection(UpdateSectionModel updateSectionModel)
        {
            var response = await _sectionService.UpdateSection(updateSectionModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
    }
}
