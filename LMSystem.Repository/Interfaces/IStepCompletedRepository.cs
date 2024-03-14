using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IStepCompletedRepository
    {
        public Task<ResponeModel> AddOrUpdateStepCompleted(int registrationId, int stepId );
        public Task<ResponeModel> GetStepIdByRegistrationId(int registrationId);
    }
}
