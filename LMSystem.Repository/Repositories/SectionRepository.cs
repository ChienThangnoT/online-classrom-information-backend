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
    public class SectionRepository : ISectionRepository
    {
        private readonly LMOnlineSystemDbContext _context;

        public SectionRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<ResponeModel> AddSection(AddSectionModel addSectionModel)
        {
            try
            {
                var section = new Section
                {
                    CourseId = addSectionModel.CourseId,
                    Title = addSectionModel.Title,
                    Position = addSectionModel.Position
                };
                _context.Sections.Add(section);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Added section successfully", DataObject = section };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the section" };
            }
        }
        //public async Task<ResponeModel> UpdateSection(UpdateSectionModel updateSectionModel)
        //{
        //    try
        //    {
        //        var section = await _context.Sections.FirstOrDefaultAsync(x => x.SectionId == updateSectionModel.SectionId);
        //        if (section == null)
        //        {
        //            return new ResponeModel { Status = "Error", Message = "Section not found" };
        //        }
        //        section = submitSectionChanges(section, updateSectionModel);

        //        await _context.SaveChangesAsync();

        //        return new ResponeModel { Status = "Success", Message = "Updated section successfully", DataObject = section };
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception: {ex.Message}");
        //        return new ResponeModel { Status = "Error", Message = "An error occurred while updating the section" };
        //    }
        //}

        private Section submitSectionChanges(Section section, UpdateSectionModel updateSectionModel)
        {
            section.Title = updateSectionModel.Title;
            section.Position = updateSectionModel.Position;

            return section;
        }
        public async Task<ResponeModel> UpdateSection(UpdateSectionModel updateSectionModel)
        {
            try
            {
                var section = await _context.Sections.FirstOrDefaultAsync(x => x.SectionId == updateSectionModel.SectionId);
                if (section == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Section not found" };
                }
                section = submitSectionChanges(section, updateSectionModel);

                await _context.SaveChangesAsync();

                var response = await _context.Sections
                    .Where(section => section.SectionId == updateSectionModel.SectionId)
                    .Include(section => section.Steps)
                    .Select(section => new
                    {
                        section.SectionId,
                        section.CourseId,
                        section.Title,
                        section.Position,
                        Steps = section.Steps.Select(step => new
                        {
                            step.StepId,
                            step.QuizId,
                            step.Title,
                            step.Position,
                            step.Duration,
                            step.StepDescription,
                            step.VideoUrl,
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();


                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Updated section successfully",
                    DataObject = response
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while updating the section" };
            }
        }

        public async Task<ResponeModel> GetSectionsByCourseId(int courseId)
        {
            try
            {
                var sections = await _context.Sections
                    .Where(s => s.CourseId == courseId)
                    .OrderBy(s => s.Position)
                    .Select(s => new
                    {
                        s.SectionId,
                        s.Title,
                        s.Position
                    })
                    .ToListAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Sections retrieved successfully",
                    DataObject = sections
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while retrieve sections list" };
            }
        }

        public async Task<ResponeModel> DeleteSection(int sectionId)
        {
            try
            {
                var sectionToDelete = await _context.Sections
                    .Include(s => s.Steps)
                    .FirstOrDefaultAsync(s => s.SectionId == sectionId);

                if (sectionToDelete == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "Section not found"
                    };
                }

                _context.Steps.RemoveRange(sectionToDelete.Steps);
                _context.Sections.Remove(sectionToDelete);
                await _context.SaveChangesAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Section deleted successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while deleting the section"
                };
            }
        }

        //private Section submitSectionChanges(Section section, UpdateSectionModel updateSectionModel)
        //{
        //    section.Title = updateSectionModel.Title;
        //    section.Position = updateSectionModel.Position;

        //    return section;
        //}
    }
}
