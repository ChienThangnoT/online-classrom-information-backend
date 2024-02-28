using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
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

                return new ResponeModel { Status = "Success", Message = "Added category successfully" , DataObject = category};
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the category" };
            }
        }
    }
}
