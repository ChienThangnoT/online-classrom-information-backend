using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
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

        public async Task<ResponeModel> CountTotalOrder()
        {
            try
            {
                var totalOrders = await _context.Orders.CountAsync();
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Total orders counted successfully",
                    DataObject = totalOrders
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while counting total orders in the system" };
            }
        }

        public async Task<ResponeModel> CountTotalOrdersByStatus(string status)
        {
            try
            {
                var totalOrders = await _context.Orders
                    .Where(o => o.Status == status)
                    .CountAsync();
                return new ResponeModel
                {
                    Status = "Success",
                    Message = $"Total {status} orders counted successfully",
                    DataObject = totalOrders
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = $"An error occurred while counting total {status} orders in the system" };
            }
        }

        public async Task<ResponeModel> CountTotalOrdersByStatusUpToDate(string status,DateTime to)
        {
            try
            {
                var totalOrders = await _context.Orders
                    .Where(o => o.Status == status && o.PaymentDate <= to)
                    .CountAsync();
                return new ResponeModel
                {
                    Status = "Success",
                    Message = $"Total {status} orders in the system up to {to} counted successfully",
                    DataObject = totalOrders
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while counting total paid orders in the system" };
            }
        }

        public async Task<ResponeModel> CountOrderByStatusGroupByMonth(string status, int year)
        {
            //return json data for chart
            try
            {
                var orderCounts = await _context.Orders
                    .Where(o => o.Status == status
                            && o.PaymentDate.HasValue
                            && o.PaymentDate.Value.Year == year)
                    .GroupBy(o => o.PaymentDate.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Count() })
                    .OrderBy(g => g.Month)
                    .ToListAsync();

                int[] array = new int[12];

                foreach (var orderCount in orderCounts)
                {
                    array[orderCount.Month - 1] = orderCount.Total;
                }

                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var jsonData = JsonConvert.SerializeObject(array, jsonSerializerSettings);

                return new ResponeModel
                {
                    Status = "Success",
                    Message = $"{status} order counts for each month in {year} retrieved successfully",
                    DataObject = jsonData
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while retrieving order counts",
                };
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(string accountId)
        {
            return await _context.Orders
                .Where(order => order.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<ResponeModel> GetYearList()
        {
            try
            {
                var distinctYears = await _context.Orders
                    .Where(o => o.PaymentDate.HasValue)
                    .Select(o => o.PaymentDate.Value.Year)
                    .Distinct()
                    .OrderByDescending(year => year)
                    .ToListAsync();

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Distinct years retrieved successfully",
                    DataObject = distinctYears
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while retrieving distinct years"
                };
            }
        }

        public async Task<ResponeModel> CountTotalIncome()
        {
            try
            {
                var totalIncome = await _context.Orders
                    .Where(o => o.Status == OrderStatusEnum.Completed.ToString()
                            && o.PaymentDate.HasValue)
                    .SumAsync(o => o.TotalPrice);

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Total income calculated successfully",
                    DataObject = totalIncome
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while calculating total income",
                };
            }
        }

        public async Task<ResponeModel> CountTotalIncomeUpToDate(DateTime to)
        {
            try
            {
                var totalIncome = await _context.Orders
                    .Where(o => o.Status == OrderStatusEnum.Completed.ToString() 
                            && o.PaymentDate.HasValue 
                            && o.PaymentDate.Value <= to)
                    .SumAsync(o => o.TotalPrice);

                return new ResponeModel
                {
                    Status = "Success",
                    Message = $"Total income up to {to} calculated successfully",
                    DataObject = totalIncome
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while calculating total income",
                };
            }
        }

        public async Task<ResponeModel> CountTotalIncomeByMonth(int year)
        {
            //return json data for chart
            try
            {
                var totalIncomeByMonth = await _context.Orders
                    .Where(o => o.Status == OrderStatusEnum.Completed.ToString()
                            && o.PaymentDate.HasValue 
                            && o.PaymentDate.Value.Year == year)
                    .GroupBy(o => o.PaymentDate.Value.Month)
                    .Select(g => new { Month = g.Key, TotalPrice = g.Sum(o => o.TotalPrice) })
                    .OrderBy(g => g.Month)
                    .ToListAsync();

                double?[] array = new double?[12]; 

                foreach (var incomeByMonth in totalIncomeByMonth)
                {
                    array[incomeByMonth.Month - 1] = incomeByMonth.TotalPrice;
                }

                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var jsonData = JsonConvert.SerializeObject(array, jsonSerializerSettings);

                return new ResponeModel
                {
                    Status = "Success",
                    Message = $"Total income for each month in {year} retrieved successfully",
                    DataObject = jsonData
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "An error occurred while retrieving total income by month",
                    DataObject = string.Empty
                };
            }
        }
    }
}
