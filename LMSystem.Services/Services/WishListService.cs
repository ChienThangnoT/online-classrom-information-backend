using LMSystem.Repository.Data;
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
    public class WishListService : IWishListService
    {
        private readonly IWishListRepository _wishListRepository;

        public WishListService(IWishListRepository wishListRepository)
        {
            _wishListRepository = wishListRepository;
        }

        public async Task<ResponeModel> AddWishList(string accountId, int courseId)
        {
            return await _wishListRepository.AddWishList(accountId, courseId);
        }

        public async Task<ResponeModel> CountTotalWishListByAccountId(string accountId)
        {
            return await _wishListRepository.CountTotalWishListByAccountId(accountId);
        }

        public async Task<ResponeModel> DeleteWishListByCourseId(int courseId, string accountId)
        {
            return await _wishListRepository.DeleteWishListByCourseId(courseId, accountId);
        }

        public async Task<ResponeModel> DeleteWishListByWishListId(int wishListId)
        {
            return await _wishListRepository.DeleteWishListByWishListId(wishListId);
        }

        public Task<List<WishListModel>> GetWishListByAccountId(string accountId)
        {
            return _wishListRepository.GetWishListByAccountId(accountId);
        }
    }
}
