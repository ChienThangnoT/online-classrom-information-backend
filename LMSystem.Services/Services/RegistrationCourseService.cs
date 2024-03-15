using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class RegistrationCourseService : IRegistrationCourseService
    {
        private readonly IRegistrationCourseRepository _registrationCourseRepository;
        public RegistrationCourseService(IRegistrationCourseRepository registrationCourseRepository)
        {
            _registrationCourseRepository = registrationCourseRepository;
        }

        public async Task<ResponeModel> GetCompletedLearningCourseByAccountId(string accountId)
        {
            return await _registrationCourseRepository.GetCompletedLearningCourseByAccountId(accountId);
        }

        public async Task<ResponeModel> GetRegisterCourseListByAccountId(string accountId)
        {
            return await _registrationCourseRepository.GetRegisterCourseListByAccountId(accountId);
        }

        public async Task<ResponeModel> CheckRegistrationCourse(string accountId, int courseId)
        {
            return await _registrationCourseRepository.CheckRegistrationCourse(accountId, courseId);
        }

        public async Task<ResponeModel> GetRegisterCourseListByParentAccountId(string accountId)
        {
            return await _registrationCourseRepository.GetRegisterCourseListByParentAccountId(accountId);
        }
        public async Task<ResponeModel> GetCompletedLearningCourseByParentAccountId(string accountId)
        {
            return await _registrationCourseRepository.GetCompletedLearningCourseByParentAccountId(accountId);
        }
        public async Task<ResponeModel> GetUncompletedLearningCourseByParentAccountId(string accountId)
        {
            return await _registrationCourseRepository.GetUncompletedLearningCourseByParentAccountId(accountId);
        }
    }
}
