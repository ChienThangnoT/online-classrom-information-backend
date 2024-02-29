using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
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
        public async Task<ResponeModel> GetRegisterCourseListByAccountId(string accountId)
        {
            return await _registrationCourseRepository.GetRegisterCourseListByAccountId(accountId);
        }
    }
}
