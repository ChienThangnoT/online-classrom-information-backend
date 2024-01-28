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

                return new ResponeModel { Status = "Success", Message = "Added section successfully",DataObject = section };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the section" };
            }
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

                return new ResponeModel { Status = "Success", Message = "Updated section successfully", DataObject = section };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while updating the section" };
            }
        }

        private Section submitSectionChanges(Section section, UpdateSectionModel updateSectionModel)
        {
            section.Title = updateSectionModel.Title;

            return section;
        }
    }
}
