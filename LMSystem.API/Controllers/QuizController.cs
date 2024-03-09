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
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("AddQuiz")]
        public async Task<IActionResult> AddQuiz([FromQuery] AddQuizModel model)
        {
            var response = await _quizService.AddQuiz(model);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
        [HttpDelete("DeleteQuiz")]
        public async Task<IActionResult> DeleteQuiz([FromQuery] int quizId)
        {
            var response = await _quizService.DeleteQuiz(quizId);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
        [HttpGet("GetAllQuiz")]
        public async Task<IActionResult> GetAllQuiz([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var response = await _quizService.GetAllQuiz(paginationParameter);
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

        [HttpPut("UpdateQuiz")]
        public async Task<IActionResult> UpdateQuiz([FromQuery] UpdateQuizModel quizModel, UpdateQuestion questionModel)
        {
            var response = await _quizService.UpdateQuiz(quizModel, questionModel);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
    }
}
