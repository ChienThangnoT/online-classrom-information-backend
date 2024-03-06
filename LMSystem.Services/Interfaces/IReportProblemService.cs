using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IReportProblemService
    {
        public Task<PagedList<ReportProblem>> GetAllReportProblem(PaginationParameter paginationParameter);

        public Task<ReportProblem> SendRequestAsync(SendRequestModel model);
        public Task<ResponeModel> ResolveRequestAsync(ResolveRequestModel model);
    }
}
