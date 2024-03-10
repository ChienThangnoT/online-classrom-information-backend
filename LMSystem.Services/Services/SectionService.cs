using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        public SectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }
        public async Task<ResponeModel> AddSection(AddSectionModel addSectionModel)
        {
            return await _sectionRepository.AddSection(addSectionModel);
        }

        public async Task<ResponeModel> DeleteSection(int sectionId)
        {
            return await _sectionRepository.DeleteSection(sectionId);
        }

        public async Task<ResponeModel> GetSectionsByCourseId(int courseId)
        {
            return await _sectionRepository.GetSectionsByCourseId(courseId);
        }

        public async Task<ResponeModel> UpdateSection(UpdateSectionModel updateSectionModel)
        {
            return await _sectionRepository.UpdateSection(updateSectionModel);
        }
    }
}
