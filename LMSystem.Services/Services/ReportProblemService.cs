using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class ReportProblemService : IReportProblemService
    {
        private readonly IReportProblemRepository _reportProblemRepository;
        public ReportProblemService(IReportProblemRepository reportProblemRepository) 
        {
            _reportProblemRepository = reportProblemRepository;            
        }

        public async Task<PagedList<ReportProblem>> GetAllReportProblem(PaginationParameter paginationParameter)
        {
            return await _reportProblemRepository.GetAllReportProblem(paginationParameter);
        }


        public async Task<ReportProblem> SendRequestAsync(SendRequestModel model)
        {
            return await _reportProblemRepository.SendRequestAsync(model);
        }

        public async Task<ResponeModel> ResolveRequestAsync(ResolveRequestModel model)
        {
            return await _reportProblemRepository.ResolveRequestAsync(model);
        }
    }
}
