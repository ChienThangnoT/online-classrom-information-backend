﻿using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class StepService : IStepService
    {
        private readonly IStepRepository _stepRepository;
        public StepService(IStepRepository stepRepository)
        {
            _stepRepository = stepRepository;
        }

        public async Task<ResponeModel> AddStep(AddStepModel addStepModel)
        {
            return await _stepRepository.AddStep(addStepModel);
        }
        public async Task<LearningProgressModel> CheckCourseProgress(int registrationId)
        {
            return await _stepRepository.CheckCourseProgress(registrationId);
        }
    }
}
