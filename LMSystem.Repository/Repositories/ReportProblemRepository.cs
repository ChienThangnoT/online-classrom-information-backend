using AutoMapper;
using LMSystem.Repository.Data;
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

        public async Task<ReportProblem> SendRequestAsync(SendRequestModel model)
        {
            var report = new ReportProblem
            {
                AccountId = model.AccountId,
                Type = model.Type,
                Title = model.Title,
                Description = model.Description,
                ReportStatus = "New",
                CreateDate = DateTime.UtcNow,
            };
            _context.ReportProblems.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<bool> ResolveRequestAsync(int reportId, string newStatus)
        {
            var report = await _context.ReportProblems.FirstOrDefaultAsync(r => r.ReportId == reportId);
            if (report == null)
            {
                return false; 
            }

            report.ReportStatus = newStatus; 
            report.ProcessingDate = DateTime.UtcNow; 

            await _context.SaveChangesAsync();
            return true; 
        }
    }
}
