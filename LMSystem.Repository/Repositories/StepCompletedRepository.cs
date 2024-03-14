using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class StepCompletedRepository : IStepCompletedRepository
    {
        private readonly LMOnlineSystemDbContext _context;

        public StepCompletedRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<ResponeModel> AddOrUpdateStepCompleted(int registrationId, int stepId)
        {
            try
            {
                //check if the registration exists or not
                var registration = await _context.RegistrationCourses
                    .FirstOrDefaultAsync(rc => rc.RegistrationId == registrationId);
                if (registration == null)
                    return new ResponeModel { Status = "Error", Message = "Registration not found" };
                //check if the step exists or not
                var step = await _context.Steps
                    .FirstOrDefaultAsync(s => s.StepId == stepId);
                if (step == null)
                    return new ResponeModel { Status = "Error", Message = "Step not found" };
                //get response
                var response = await _context.RegistrationCourses
                .FirstOrDefaultAsync(rc => rc.RegistrationId == registrationId);
                //get the registrationCourse
                var registrationCourse = await _context.RegistrationCourses
                    .FirstOrDefaultAsync(rc => rc.RegistrationId == registrationId);
                //get the total steps in the course
                var totalSteps = await _context.Steps
                    .CountAsync(s => s.Section.CourseId == registration.CourseId);
                //get position of the step
                var positionOfStep = await _context.Steps
                    .Where(step => step.StepId == stepId)
                    .Select(step => step.Position)
                    .FirstOrDefaultAsync();
                //calculate the learning progress
                var learningProgress = totalSteps > 0
                    ? (double)positionOfStep / totalSteps
                    : 0;
                //check if the course is completed or not
                var isCompleted = learningProgress == 1;
                //check if the registrationId exist in the stepCompleted table or not
                var stepCompleted = _context.StepCompleteds
                    .Where(s => s.RegistrationId == registrationId )
                    .FirstOrDefault();
                if (stepCompleted == null)
                {
                    // create new StepCompleted object
                    var newStepCompleted = new StepCompleted
                    {
                        RegistrationId = registrationId,
                        StepId = stepId,
                        DateCompleted = DateTime.Now
                    };
                    _context.StepCompleteds.Add(newStepCompleted);
                    await _context.SaveChangesAsync();                                     
                    //update the learning progress and isCompleted in the registrationCourse table
                    registrationCourse.LearningProgress = learningProgress;
                    registrationCourse.IsCompleted = isCompleted;
                    _context.Update(registrationCourse);
                    await _context.SaveChangesAsync();
                    //return the response
                    return new ResponeModel { 
                        Status = "Success", 
                        Message = "Step completed successfully" ,
                        DataObject = response
                    };
                }
                //get the position of the step in the stepCompleted table
                var positionOfStepInStepCompleted = _context.Steps
                    .Where(step => step.StepId == stepCompleted.StepId)
                    .Select(step => step.Position)
                    .FirstOrDefault();
                //check if the position of the step is less than the position of the step in the stepCompleted table
                if (positionOfStep < positionOfStepInStepCompleted)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "The position of the step is less than the position of the step in the stepCompleted table",
                        DataObject = new { positionOfStep, positionOfStepInStepCompleted }
                    };
                }

                //update the stepCompleted table
                stepCompleted.StepId = stepId;
                stepCompleted.DateCompleted = DateTime.Now;
                _context.Update(stepCompleted);
                await _context.SaveChangesAsync();
                //update the learning progress and isCompleted in the registrationCourse table
                registrationCourse.LearningProgress = learningProgress;
                registrationCourse.IsCompleted = isCompleted;
                _context.Update(registrationCourse);
                await _context.SaveChangesAsync();
                //return the response
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Step completed successfully",
                    DataObject = response
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while check step complete" };
            }
        }

        public async Task<ResponeModel> GetStepIdByRegistrationId(int registrationId)
        {
            try
            {
                var stepId = await _context.StepCompleteds
                    .Where(s => s.RegistrationId == registrationId)
                    .Select(s => s.StepId)
                    .FirstOrDefaultAsync();
                if (stepId == 0)
                    return new ResponeModel { Status = "Error", Message = "No step were found for the specified registration id" };
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Get step id successfully",
                    DataObject = new { stepId }
                };
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while get stepId by the specified registration id" };

            }
        }
    }
}
