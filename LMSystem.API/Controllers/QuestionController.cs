using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("AddQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddQuestion([FromQuery] AddQuestionModel model)
        {
            var response = await _questionService.AddQuestion(model);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
        [HttpDelete("DeleteQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuestion([FromQuery] int questionId)
        {
            var response = await _questionService.DeleteQuestion(questionId);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
        [HttpGet("GetAllQuestion")]
        public async Task<IActionResult> GetAllQuestion([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var response = await _questionService.GetAllQuestion(paginationParameter);
                var metadata = new
                {
                    response.TotalCount,
                    response.PageSize,
                    response.CurrentPage,
                    response.TotalPages,
                    response.HasNext,
                    response.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                if (!response.Any())
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateQuestion([FromQuery] UpdateQuestionModel model)
        {
            var response = await _questionService.UpdateQuestion(model);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
    }
}
