using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class StepRepository : IStepRepository
    {
        private readonly LMOnlineSystemDbContext _context;
        public StepRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }
        public async Task<ResponeModel> AddStep(AddStepModel addStepModel)
        {
            try
            {
                var step = new Step
                {
                    SectionId = addStepModel.SectionId,
                    Duration = addStepModel.Duration,
                    Position = addStepModel.Position,
                    Title = addStepModel.Title,
                    VideoUrl = addStepModel.VideoUrl,
                    StepDescription = addStepModel.StepDescription
                };
                _context.Steps.Add(step);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Added step successfully", DataObject = step };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the step" };
            }
        }

        public async Task<LearningProgressModel> CheckCourseProgress(int registrationId)
        {
            var registration = await _context.RegistrationCourses
        .FirstOrDefaultAsync(rc => rc.RegistrationId == registrationId);
            if (registration == null) return null;

            var courseId = registration.CourseId;

            // Get the IDs of completed steps
            var completedStepsInfo = await _context.StepCompleteds
                .Where(sc => sc.RegistrationId == registrationId)
                .Select(sc => new { sc.CompletedStepId, sc.DateCompleted })
                .ToListAsync();

            var completedStepIds = completedStepsInfo.Select(info => info.CompletedStepId).ToList();
            var latestCompletionDate = completedStepsInfo
                .OrderByDescending(info => info.DateCompleted)
                .Select(info => info.DateCompleted)
                .FirstOrDefault();


            // Count the total number of steps in the course
            var totalSteps = await _context.Steps
                .CountAsync(s => s.Section.CourseId == courseId);

            // Calculate the learning progress percentage
            var progressPercentage = totalSteps > 0
                ? (double)completedStepIds.Count / totalSteps
                : 0;
            
            var isComplete = progressPercentage < 1
                ? registration.IsCompleted = false : registration.IsCompleted = true;

            // Update the learning progress in the RegistrationCourse
            registration.LearningProgress = progressPercentage;
            registration.IsCompleted = isComplete;
            _context.Update(registration);
            _context.Update(registration);
            await _context.SaveChangesAsync();

            // Find the latest completed step
            var latestStep = await _context.Steps
                .Include(s => s.Section)
                .Where(s => completedStepIds.Contains(s.StepId))
                .OrderByDescending(s => s.SectionId).ThenByDescending(s => s.Position)
                .FirstOrDefaultAsync();

            return new LearningProgressModel
            {
                CurrentStepPosition = latestStep?.Position,
                CurrentStep = latestStep?.Title,
                CurrentSection = latestStep?.Section?.Title,
                ProgressPercentage = progressPercentage,
                IsCompleted = isComplete,
                EnrollDay = registration.EnrollmentDate.GetValueOrDefault(), 
                StepCompleteDay = latestCompletionDate.GetValueOrDefault()
            };
        }

    }
}
