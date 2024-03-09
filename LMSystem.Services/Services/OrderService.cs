﻿using LMSystem.Repository.Data;
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
    }
}
