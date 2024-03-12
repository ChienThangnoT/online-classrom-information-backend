using AutoMapper;
using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class ReportProblemRepository : IReportProblemRepository
    {
        private readonly LMOnlineSystemDbContext _context;

        public ReportProblemRepository(LMOnlineSystemDbContext context)

        {
            this._context = context;
        }

        public async Task<PagedList<ReportProblem>> GetAllReportProblem(PaginationParameter paginationParameter)
        {
            if (_context == null)
            {
                return null;
            }
            var request = _context.ReportProblems.AsQueryable();
            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                request = request.Where(o => o.Title.Contains(paginationParameter.Search));
            }

            var allRequest = await request.ToListAsync();

            return PagedList<ReportProblem>.ToPagedList(allRequest,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<ReportProblem> SendRequestAsync(SendRequestModel model)
        {
            var report = new ReportProblem
            {
                AccountId = model.AccountId,
                Type = model.Type.ToString(),
                Title = model.Title,
                Description = model.Description,
                ReportStatus = ReportStatus.Pending.ToString(),
                CreateDate = DateTime.UtcNow,
            };
            _context.ReportProblems.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<ResponeModel> ResolveRequestAsync(ResolveRequestModel model)
        {
            try
            {
                var report = await _context.ReportProblems.FirstOrDefaultAsync(r => r.ReportId == model.ReportId);
                if (report == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Request not found" };
                }

                report.ReportStatus = model.Status.ToString();
                report.ProcessingDate = model.newProCessingDay;

                await _context.SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Request updated successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while resolve the request" };
            }
        }


    }
}
