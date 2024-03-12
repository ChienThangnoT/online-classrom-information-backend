using AutoMapper;
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
    public class WishListRepository : IWishListRepository
    {
        private readonly LMOnlineSystemDbContext _context;
        private readonly IMapper _mapper;

        public WishListRepository(LMOnlineSystemDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponeModel> AddWishList(string accountId, int courseId)
        {
            try
            {
                if (_context.WishLists.Any(w => w.AccountId == accountId && w.CourseId == courseId))
                {
                    return new ResponeModel { Status = "Error", Message = "Course already exists in the wishlist" };
                }
                var wishlistItem = new Data.WishListModel
                {
                    AccountId = accountId,
                    CourseId = courseId
                };

                _context.WishLists.Add(_mapper.Map<Models.WishList>(wishlistItem));
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Course added to the wishlist successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the course to the wishlist" };
            }
        }

        public async Task<ResponeModel> CountTotalWishListByAccountId(string accountId)
        {
            try
            {
                var totalWishListItems = await _context.WishLists
                    .Where(w => w.AccountId == accountId)
                    .CountAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Total number of wishlist items retrieved successfully",
                    DataObject = totalWishListItems
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while retrieving total number of wishlist items"
                };
            }
        }

        public async Task<ResponeModel> DeleteWishListByCourseId(int courseId, string accountId)
        {
            try
            {
                var existingWishList = await _context.WishLists
                    .FirstOrDefaultAsync(w => w.CourseId == courseId && w.AccountId == accountId);

                if (existingWishList == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No wishlist items were found for the specified course id"
                    };
                }

                _context.WishLists.RemoveRange(existingWishList);
                await _context.SaveChangesAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Course deleted from the wishlist successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while deleting the wishlist item" };
            }
        }

        public async Task<ResponeModel> DeleteWishListByWishListId(int wishListId)
        {
            try
            {
                var existingWishList = await _context.WishLists.FirstOrDefaultAsync(w => w.WishListId == wishListId);

                if (existingWishList == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No wishlist items were found for the specified wishlist id"
                    };
                }

                _context.WishLists.Remove(existingWishList);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Course deleted from the wishlist successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while deleting the wishlist item" };
            }
        }

        public async Task<List<WishListModel>> GetWishListByAccountId(string accountId)
        {
            try
            {
                var wishlistItems = await _context.WishLists
                    .Where(w => w.AccountId == accountId)
                    .ToListAsync();

                return _mapper.Map<List<WishListModel>>(wishlistItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
    }
}
