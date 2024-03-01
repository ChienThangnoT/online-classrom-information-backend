using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMSystem.Repository.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        // Assuming you have a DbContext or similar for data access
        private readonly LMOnlineSystemDbContext _context;

        public OrderRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(string accountId)
        {
            return await _context.Orders
                .Where(order => order.AccountId == accountId)
                .ToListAsync();
        }
    }
}
