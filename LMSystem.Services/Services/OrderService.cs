using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ResponeModel> AddCourseToPayment(AddOrderPaymentModel addOrderPaymentModel)
        {
            return await _orderRepository.AddCourseToPayment(addOrderPaymentModel);

        }

        public async Task<ResponeModel> CountOrderByStatusGroupByMonth(string status,int year)
        {
            return await _orderRepository.CountOrderByStatusGroupByMonth(status,year);
        }

        public async Task<ResponeModel> CountTotalIncome()
        {
            return await _orderRepository.CountTotalIncome();
        }

        public async Task<ResponeModel> CountTotalIncomeByMonth(int year)
        {
            return await _orderRepository.CountTotalIncomeByMonth(year);
        }

        public async Task<ResponeModel> CountTotalIncomeUpToDate(DateTime to)
        {
            return await _orderRepository.CountTotalIncomeUpToDate(to);
        }

        public async Task<ResponeModel> CountTotalOrder()
        {
            return await _orderRepository.CountTotalOrder();
        }

        public async Task<ResponeModel> CountTotalOrdersByStatus(string status)
        {
            return await _orderRepository.CountTotalOrdersByStatus(status);
        }

        public async Task<ResponeModel> CountTotalOrdersByStatusUpToDate(string status,DateTime to)
        {
            return await _orderRepository.CountTotalOrdersByStatusUpToDate(status,to);
        }

        public async Task<ResponeModel> GetOrderSuccessByAccountIdAndCourseId(string accountId, int courseId)
        {
            return await _orderRepository.GetOrderSuccessByAccountIdAndCourseId(accountId, courseId);
        } 
        
        public async Task<ResponeModel> GetOrderPendingByAccountIdAndCourseId(string accountId, int courseId)
        {
            return await _orderRepository.GetOrderPendingByAccountIdAndCourseId(accountId, courseId);
        }

        public async Task<IEnumerable<Order>> GetOrderHistoryAsync(string accountId)
        {
            var orders = await _orderRepository.GetOrdersByAccountIdAsync(accountId);
            // Map to DTOs if necessary, and apply any business logic (e.g., sorting)
            return orders.Select(order => new Order
            {
                // Assuming you have an OrderDto to map the necessary fields
                OrderId = order.OrderId,
                CourseId = order.CourseId,
                TotalPrice = order.TotalPrice,
                PaymentMethod = order.PaymentMethod,
                TransactionNo = order.TransactionNo,
                PaymentDate = order.PaymentDate,
                CurrencyCode = order.CurrencyCode,
                AccountName = order.AccountName,
                Status = order.Status
            });
        }

        public async Task<ResponeModel> GetYearList()
        {
            return await _orderRepository.GetYearList();
        }
    }
}
