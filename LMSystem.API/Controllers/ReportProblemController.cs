using AutoMapper.Internal;
using Azure;
using Humanizer;
using LMSystem.API.Helper;
using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Composition;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportProblemController : ControllerBase
    {
        private readonly IReportProblemService _reportProblemService;

        public ReportProblemController(IReportProblemService reportProblemService)
        {
            _reportProblemService = reportProblemService;

        }

        [HttpGet("GetAllRequest")]
        [Authorize]
        public async Task<IActionResult> GetAllRequest([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var response = await _reportProblemService.GetAllReportProblem(paginationParameter);
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

        [HttpPost("SendRequest")]
        [Authorize]
        public async Task<IActionResult> SendRequest([FromQuery] SendRequestModel model)
        {
            var report = await _reportProblemService.SendRequestAsync(model);
            return Ok(report);
        }

        [HttpPut("ResolveRequest")]
        public async Task<IActionResult> ResolveRequest(ResolveRequestModel model)
        {
            var result = await _reportProblemService.ResolveRequestAsync(model);
            if (result.Status == "Error")
            {
                return Conflict(result);
            }

            return Ok(result);
        }
    }
}
