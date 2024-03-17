using LMSystem.Repository.Data;
using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSection(UpdateSectionModel updateSectionModel)
        {
            var response = await _sectionService.UpdateSection(updateSectionModel);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
        [HttpGet("GetSectionsByCourseId")]
        public async Task<IActionResult> GetSectionsByCourseId(int courseId)
        {
            var response = await _sectionService.GetSectionsByCourseId(courseId);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteSection")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSection(int sectionId)
        {
            var response = await _sectionService.DeleteSection(sectionId);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
