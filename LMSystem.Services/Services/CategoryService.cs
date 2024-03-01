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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) 
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponeModel> AddCategory(AddCategoryModel model)
        {
            return await _categoryRepository.AddCategory(model);
        }

        public async Task<ResponeModel> DeleteCategory(int categoryId)
        {
            return await _categoryRepository.DeleteCategory(categoryId);
        }

        public async Task<ResponeModel> GetAllCategory()
        {
            return await _categoryRepository.GetAllCategory();
        }

        public async Task<ResponeModel> UpdateCategory(UpdateCategoryModel model)
        {
            return await _categoryRepository.UpdateCategory(model);
        }
    }
}
