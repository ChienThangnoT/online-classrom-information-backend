using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface ISectionService
    {
        public Task<ResponeModel> AddSection(AddSectionModel addSectionModel);
        public Task<ResponeModel> UpdateSection(UpdateSectionModel updateSectionModel);
    }
}
