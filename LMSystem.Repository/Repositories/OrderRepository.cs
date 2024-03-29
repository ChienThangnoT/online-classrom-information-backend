﻿using LMSystem.Repository.Data;
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
using AutoMapper;
using System.Globalization;
using Microsoft.SqlServer.Server;
using LMSystem.Repository.Helpers;
using System.Net.NetworkInformation;

namespace LMSystem.Repository.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        // Assuming you have a DbContext or similar for data access
        private readonly LMOnlineSystemDbContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public OrderRepository(LMOnlineSystemDbContext context, IAccountRepository accountRepository, ICourseRepository courseRepository, IMapper mapper)
        {
            _context = context;
            _accountRepository = accountRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
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

        public async Task<ResponeModel> CountTotalOrdersByStatusUpToDate(string status, DateTime to)
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

        #region  payment
        public async Task<ResponeModel> AddCourseToPayment(AddOrderPaymentModel addOrderPaymentModel)
        {
            try
            {
                var account = await _accountRepository.GetAccountById(addOrderPaymentModel.AccountId);
                if (account == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Account is not exist" };
                }
                var course = await _courseRepository.GetCourseDetailByIdAsync(addOrderPaymentModel.CourseId);
                if (course == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Course is not exist" };
                }
                var checkOrderSuccess = await GetOrderSuccessByAccountIdAndCourseId(addOrderPaymentModel.AccountId, addOrderPaymentModel.CourseId);
                var checkOrderPending = await GetOrderPendingByAccountIdAndCourseId(addOrderPaymentModel.AccountId, addOrderPaymentModel.CourseId);

                if (checkOrderSuccess.Status == "Error" && checkOrderPending.Status == "Error")
                {
                    var status = new OrderPaymentModel();
                    var totalPrice = Math.Round((double)((course.Price - (course.Price * course.SalesCampaign)) / 24640), 2);
                    //var formattedTotalPrice = totalPrice.ToString("0.00", CultureInfo.InvariantCulture);


                    var Order = new Order
                    {
                        AccountId = account.Id,
                        AccountName = account.FirstName + " " + account.LastName,
                        CourseId = addOrderPaymentModel.CourseId,
                        TotalPrice = totalPrice,
                        PaymentDate = DateTime.Now,
                        Status = status.Status,
                    };
                    _context.Add(Order);
                    _context.SaveChanges();
                    return new ResponeModel
                    {
                        Status = "Success",
                        Message = "Add Order to Database success!",
                        DataObject = _mapper.Map<OrderPaymentModel>(Order)

                    };
                }
                else if (checkOrderSuccess.Status == "Error" && checkOrderPending.Status != "Error")
                {

                    return new ResponeModel
                    {
                        Status = "Success",
                        Message = "Find Order was pending!",
                        DataObject = _mapper.Map<OrderPaymentModel>(checkOrderPending.DataObject)
                    };
                }
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "Order has been completed!",
                };

            }
            catch (Exception ex)
            {
                return new ResponeModel
                {
                    Status = "Error",
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponeModel> GetOrderSuccessByAccountIdAndCourseId(string accountId, int courseId)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(t => t.AccountId == accountId && t.CourseId == courseId && t.Status == OrderStatusEnum.Completed.ToString());
                if (order == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "Not found order has completed!"
                    }; ;
                }
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Find order has completed!",
                    DataObject = order
                };
            }
            catch (Exception ex)
            {
                return new ResponeModel
                {
                    Status = "Error",
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponeModel> GetOrderPendingByAccountIdAndCourseId(string accountId, int courseId)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(t => t.AccountId == accountId && t.CourseId == courseId && t.Status == OrderStatusEnum.Pending.ToString());
                if (order == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "Not found order has pending!"
                    }; ;
                }
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Find order was pending!",
                    DataObject = order
                };
            }
            catch (Exception ex)
            {
                return new ResponeModel
                {
                    Status = "Error",
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponeModel> GetOrderByTransactionId(string transactionId)
        {
            try
            {
                var transaction = await _context.Orders.FirstOrDefaultAsync(t => t.TransactionNo == transactionId);
                if (transaction == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "Not found order has transactionId!"
                    };
                }
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Find order was pending!",
                    DataObject = transaction
                };
            }
            catch (Exception ex)
            {
                return new ResponeModel
                {
                    Status = "Error",
                    Message = ex.Message
                };
            }
        }
        #endregion
        public async Task<PagedList<Order>> GetOrderWithFilter(PaginationParameter paginationParameter, OrderFilterParameter orderFilterParameter)
        {
            try
            {
                var query = _context.Orders.AsQueryable();

                if (!string.IsNullOrEmpty(orderFilterParameter.Status))
                {
                    if (!Enum.TryParse(orderFilterParameter.Status, out OrderStatusEnum status))
                    {
                        return new PagedList<Order>(new List<Order>(), 0, 0, 0);
                    }
                    switch (orderFilterParameter.Status)
                    {
                        case "Completed":
                            query = query.Where(o =>
                            o.Status == OrderStatusEnum.Completed.ToString());
                            break;
                        case "Failed":
                            query = query.Where(o =>
                            o.Status == OrderStatusEnum.Failed.ToString());
                            break;
                        case "Pending":
                            query = query.Where(o =>
                            o.Status == OrderStatusEnum.Pending.ToString());
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(orderFilterParameter.AccountId))
                {
                    query = query.Where(o => o.AccountId == orderFilterParameter.AccountId);
                    bool accountExists = await _context.Account.AnyAsync(a => a.Id == orderFilterParameter.AccountId);
                    if (!accountExists)
                    {
                        // Return empty list if account ID doesn't exist
                        return new PagedList<Order>(new List<Order>(), 0, 0, 0);
                    }
                }
          

                var orders = await query.OrderBy(o => o.OrderId).ToListAsync();

                return PagedList<Order>.ToPagedList(orders, paginationParameter.PageNumber, paginationParameter.PageSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new PagedList<Order>(new List<Order>(), 0, 0, 0);
            }
        }

    }
}

