using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LMOnlineSystemDbContext _context;
        public CategoryRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }
        public async Task<ResponeModel> AddCategory(AddCategoryModel model)
        {
            try
            {
                var category = new Category
                {
                    Name = model.CategoryName,
                    Description = model.CategoryDescription
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Added category successfully", DataObject = category };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the category" };
            }
        }

        public async Task<ResponeModel> DeleteCategory(int categoryId)
        {
            try
            {
                var categoryToDelete = await _context.Categories
                    .Include(c => c.CourseCategories)
                    .FirstOrDefaultAsync(c => c.CatgoryId == categoryId);

                if (categoryToDelete == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Category not found" };
                }

                // Remove CourseCategory associations
                _context.CourseCategories.RemoveRange(categoryToDelete.CourseCategories);

                // Remove the category itself
                _context.Categories.Remove(categoryToDelete);

                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Delete category successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while delete the category" };
            }
        }

        public async Task<PagedList<Category>> GetAllCategory(PaginationParameter paginationParameter)
        {
            if (_context == null)
            {
                return null;
            }
            var categories = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                categories = categories.Where(o => o.Name.Contains(paginationParameter.Search));
            }

            var allCategories = await categories.ToListAsync();

            return PagedList<Category>.ToPagedList(allCategories,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<ResponeModel> UpdateCategory(UpdateCategoryModel model)
        {
            try
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CatgoryId == model.CategoryId);
                if (existingCategory == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Category not found" };
                }
                // Update category
                existingCategory.Name = model.CategoryName;
                existingCategory.Description = model.CategoryDescription;

                //_context.Courses.Update(existingCategory);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Update course successfully", DataObject = existingCategory };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while update the course" };
            }
        }
    }
}
