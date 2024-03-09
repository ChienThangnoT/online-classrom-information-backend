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

    }
}
