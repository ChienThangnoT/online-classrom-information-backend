using LMSystem.Library;
using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly PaypalClient _paypalClient;
        private readonly LMOnlineSystemDbContext _context;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICourseRepository _courseRepository;

        public OrderService(IOrderRepository orderRepository, ICourseRepository courseRepository, PaypalClient paypalClient, LMOnlineSystemDbContext context, INotificationRepository notificationRepository)
        {
            _orderRepository = orderRepository;
            _paypalClient = paypalClient;
            _context = context;
            _notificationRepository = notificationRepository;
            _courseRepository = courseRepository;
        }

        public async Task<ResponeModel> AddCourseToPayment(AddOrderPaymentModel addOrderPaymentModel)
        {
            return await _orderRepository.AddCourseToPayment(addOrderPaymentModel);

        }

        public async Task<ResponeModel> CountOrderByStatusGroupByMonth(string status, int year)
        {
            return await _orderRepository.CountOrderByStatusGroupByMonth(status, year);
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

        public async Task<ResponeModel> CountTotalOrdersByStatusUpToDate(string status, DateTime to)
        {
            return await _orderRepository.CountTotalOrdersByStatusUpToDate(status, to);
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


        public async Task<ResponeModel> CreatePaymentWithPayPal(string accountId, int courseId)
        {
            var orderResponse = await GetOrderPendingByAccountIdAndCourseId(accountId, courseId);
            //init infor order to send to paypal
            if (orderResponse.Status != "Success")
            {
                return orderResponse;
            }

            var order = (Order)orderResponse.DataObject;
            var value = order.TotalPrice.ToString().Replace(",", ".");
            var currency = "USD";
            var referenceId = order.OrderId + DateTime.Now.Ticks.ToString();
            try
            {
                var result = await _paypalClient.CreateOrder(value, currency, referenceId);
                order.TransactionNo = result.id;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Create order",
                    DataObject = result
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

        public async Task<ResponeModel> CreateCapturetWithPayPal(string transactionId)
        {
            try
            {
                var result = await _paypalClient.CaptureOrder(transactionId);
                var order = await GetOrderByTransactionId(transactionId); // Giả định bạn có phương thức này

                var orders = (Order)order.DataObject;
                orders.PaymentDate = ConvertToLocalTime(DateTime.UtcNow);
                orders.PaymentMethod = "PayPal";
                orders.CurrencyCode = "USD";
                orders.Status = OrderStatusEnum.Completed.ToString();
                _context.Orders.Update(orders);
                await _context.SaveChangesAsync();

                var course = await _courseRepository.GetCourseDetailByIdAsync(orders.CourseId);

                var registration = new RegistrationCourse
                {
                    AccountId = orders.AccountId,
                    CourseId = orders.CourseId,
                    EnrollmentDate = ConvertToLocalTime(DateTime.UtcNow),
                    IsCompleted = false,
                    LearningProgress = 0
                };
                _context.RegistrationCourses.Add(registration);
                await _context.SaveChangesAsync();

                Notification notification = new Notification
                {
                    AccountId = registration.AccountId,
                    SendDate = ConvertToLocalTime(DateTime.UtcNow),
                    Type = NotificationType.Order.ToString(),
                    Title = $"Bạn đã thanh toán thành công khóa học {course.Title}",
                    Message = "Cảm ơn bạn đã tin tưởng lựa chọn eStudyHub. Hãy trải nghiệm khóa học để có kiến thức bổ ích!",
                    ModelId = course.CourseId
                };
                await _notificationRepository.AddNotificationByAccountId(notification.AccountId, notification);

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Capture order success",
                    DataObject = result
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

        private DateTime ConvertToLocalTime(DateTime utcDateTime)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
        }

        public async Task<ResponeModel> GetOrderByTransactionId(string transactionId)
        {
            return await _orderRepository.GetOrderByTransactionId(transactionId);
        }

        public async Task<PagedList<Order>> GetOrderWithFilter(PaginationParameter paginationParameter, OrderFilterParameter orderFilterParameter)
        {
            return await _orderRepository.GetOrderWithFilter(paginationParameter, orderFilterParameter);
        }
    }
}
