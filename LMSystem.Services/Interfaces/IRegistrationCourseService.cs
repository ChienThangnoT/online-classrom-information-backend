using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IRegistrationCourseService
    {
        public Task<ResponeModel> GetRegisterCourseListByAccountId(string accountId);
        public Task<ResponeModel> GetCompletedLearningCourseByAccountId(string accountId);
    }
}
