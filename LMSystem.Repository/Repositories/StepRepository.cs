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

        public async Task<ResponeModel> UpdateStep(UpdateStepModel updateStepModel)
        {
            try
            {
                var step = await _context.Steps.FirstOrDefaultAsync(x => x.StepId == updateStepModel.StepId);
                if (step == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Step not found" };
                }
                step = submitStepChanges(step, updateStepModel);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Updated step successfully", DataObject = step };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while updating the step" };
            }
        }

        private Step submitStepChanges(Step step, UpdateStepModel updateStepModel)
        {
            step.Duration = updateStepModel.Duration;
            step.Title = updateStepModel.Title;
            step.VideoUrl = updateStepModel.VideoUrl;
            step.StepDescription = updateStepModel.StepDescription;

            return step;
        }   
    }
}
