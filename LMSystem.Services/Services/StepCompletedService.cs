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
    public class StepCompletedService : IStepCompletedService
    {
        private readonly IStepCompletedRepository _stepCompletedRepository;
        public StepCompletedService(IStepCompletedRepository stepCompletedRepository)
        {
            _stepCompletedRepository = stepCompletedRepository;
        }

        public async Task<ResponeModel> AddOrUpdateStepCompleted(int registrationId, int stepId)
        {
            return await _stepCompletedRepository.AddOrUpdateStepCompleted(registrationId, stepId);
        }

        public async Task<ResponeModel> GetStepIdByRegistrationId(int registrationId)
        {
            return await _stepCompletedRepository.GetStepIdByRegistrationId(registrationId);
        }
    }
}
