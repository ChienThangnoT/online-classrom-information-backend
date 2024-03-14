using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IStepCompletedService
    {
        public Task<ResponeModel> AddOrUpdateStepCompleted(int registrationId, int stepId );
        public Task<ResponeModel> GetStepIdByRegistrationId(int registrationId);
    }
}
