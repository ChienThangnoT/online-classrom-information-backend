using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
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

        public async Task<PagedList<Category>> GetAllCategory(PaginationParameter paginationParameter)
        {
            return await _categoryRepository.GetAllCategory(paginationParameter);
        }

        public async Task<ResponeModel> UpdateCategory(UpdateCategoryModel model)
        {
            return await _categoryRepository.UpdateCategory(model);
        }
    }
}
