using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IWishListService
    {
        public Task<ResponeModel> AddWishList(string accountId, int courseId);
        public Task<ResponeModel> DeleteWishListByWishListId(int wishListId);
        public Task<ResponeModel> DeleteWishListByCourseId(int courseId, string accountId);
        public Task<List<WishListModel>> GetWishListByAccountId(string accountId);
        public Task<ResponeModel> CountTotalWishListByAccountId(string accountId);
    }
}
