using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<Order>> GetOrderHistoryAsync(string accountId);
        public Task<ResponeModel> CountTotalOrder();
        public Task<ResponeModel> CountTotalOrdersByStatus(string status);
        public Task<ResponeModel> CountTotalOrdersByStatusUpToDate(string status,DateTime to);
        public Task<ResponeModel> CountOrderByStatusGroupByMonth(string status,int year);
        public Task<ResponeModel> GetYearList();
        public Task<ResponeModel> CountTotalIncome();
        public Task<ResponeModel> CountTotalIncomeUpToDate(DateTime to);
        public Task<ResponeModel> CountTotalIncomeByMonth(int year);

        #region  payment
        Task<ResponeModel> AddCourseToPayment(AddOrderPaymentModel addOrderPaymentModel);
        Task<ResponeModel> GetOrderSuccessByAccountIdAndCourseId(string accountId, int courseId);
        Task<ResponeModel> GetOrderPendingByAccountIdAndCourseId(string accountId, int courseId);

        #endregion
    }
}
