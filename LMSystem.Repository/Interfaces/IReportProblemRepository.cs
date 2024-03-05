using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IReportProblemRepository
    {
        public Task<ReportProblem> SendRequestAsync(SendRequestModel model);
        public Task<bool> ResolveRequestAsync(int reportId, string newStatus);

    }
}
