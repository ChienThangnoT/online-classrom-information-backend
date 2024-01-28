using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IStepRepository
    {
        public Task<ResponeModel> AddStep(AddStepModel addStepModel);
        public Task<ResponeModel> UpdateStep(UpdateStepModel updateStepModel);
    }
}
