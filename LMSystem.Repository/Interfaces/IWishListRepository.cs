using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IWishListRepository
    {
        public Task<ResponeModel> AddWishList(string accountId, string courseId);
        public Task<ResponeModel> DeleteWishListByWishListId(string wishListId);
        public Task<ResponeModel> DeleteWishListByCourseId(string courseId, string accountId);
        public Task<List<WishListModel>> GetWishListByAccountId(string accountId);
    }
}
