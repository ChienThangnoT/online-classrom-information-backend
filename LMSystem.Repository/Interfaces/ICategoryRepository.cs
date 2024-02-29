using Azure;
using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ResponeModel> AddCategory(AddCategoryModel model);
        Task<ResponeModel> UpdateCategory(UpdateCategoryModel model);
        Task<ResponeModel> DeleteCategory(int categoryId);
        Task<ResponeModel> GetAllCategory();
    }
}
