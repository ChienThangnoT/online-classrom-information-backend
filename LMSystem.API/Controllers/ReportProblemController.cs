using AutoMapper.Internal;
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

        [HttpPost("SendRequest")]
        public async Task<IActionResult> SendRequest([FromQuery] SendRequestModel model)
        {
            var report = await _reportProblemService.SendRequestAsync(model);
            return Ok(report);
        }

        [HttpPatch("ResolveRequest/{reportId}")]
        public async Task<IActionResult> ResolveRequest(int reportId, [FromQuery] string newStatus)
        {
            var result = await _reportProblemService.ResolveRequestAsync(reportId, newStatus);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
