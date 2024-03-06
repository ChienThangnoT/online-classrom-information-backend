using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(string accountId);
        public Task<ResponeModel> CountTotalOrder();
        public Task<ResponeModel> CountTotalPaidOrders();
        public Task<ResponeModel> CountTotalPaidOrdersUpToDate(DateTime to);
        public Task<ResponeModel> CountOrderPaidByMonth(int year);
        public Task<ResponeModel> GetYearList();
        public Task<ResponeModel> CountTotalIncome();
        public Task<ResponeModel> CountTotalIncomeUpToDate(DateTime to);
        public Task<ResponeModel> CountTotalIncomeByMonth(int year);
    }
}
