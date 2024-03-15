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
        public Task<ResponeModel> CheckRegistrationCourse(string accountId, int courseId);
        public Task<ResponeModel> GetRegisterCourseListByParentAccountId(string accountId);
        public Task<ResponeModel> GetCompletedLearningCourseByParentAccountId(string accountId);
        public Task<ResponeModel> GetUncompletedLearningCourseByParentAccountId(string accountId);

    }
}
