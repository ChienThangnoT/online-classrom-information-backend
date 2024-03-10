using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IStepService
    {
        public Task<ResponeModel> AddStep(AddStepModel addStepModel);
        public Task<ResponeModel> UpdateStep(UpdateStepModel updateStepModel);
        public Task<ResponeModel> DeleteStep(int stepId);
        public Task<LearningProgressModel> CheckCourseProgress(int registrationId);
        public Task<ResponeModel> GetStepsBySectionId(int sectionId);

    }
}
