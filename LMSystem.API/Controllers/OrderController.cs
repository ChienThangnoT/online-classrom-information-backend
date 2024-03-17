using LMSystem.Library;
using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly PaypalClient _paypalClient;

        public OrderController(IOrderService orderRepository, PaypalClient paypalClient)
        {
            _orderService = orderRepository;
            _paypalClient = paypalClient;
        }

        [HttpGet("PaymentHistory")]
        [Authorize]
        public async Task<IActionResult> GetOrderHistory([FromQuery] string accountId)
        {
            var orderHistory = await _orderService.GetOrderHistoryAsync(accountId);
            if (!orderHistory.Any())
            {
                return NotFound("No orders found for the account.");
            }
            return Ok(orderHistory);
        }
        [HttpGet("TotalOrders")]
        [Authorize]
        public async Task<IActionResult> CountTotalOrder()
        {
            var response = await _orderService.CountTotalOrder();
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountTotalOrdersByStatus")]
        public async Task<IActionResult> CountTotalOrdersByStatus([FromQuery] string status)
        {
            var response = await _orderService.CountTotalOrdersByStatus(status);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountTotalOrdersByStatusUpToDate")]
        public async Task<IActionResult> CountTotalOrdersByStatusUpToDate([FromQuery] string status,[FromQuery] DateTime to)
        {
            var response = await _orderService.CountTotalOrdersByStatusUpToDate(status, to);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("CountOrderByStatusGroupByMonth")]
        public async Task<IActionResult> CountOrderByStatusGroupByMonth([FromQuery] string status, [FromQuery] int year)
        {
            var response = await _orderService.CountOrderByStatusGroupByMonth(status, year);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("YearList")]
        public async Task<IActionResult> GetYearList()
        {
            var response = await _orderService.GetYearList();
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("TotalIncome")]
        public async Task<IActionResult> CountTotalIncome()
        {
            var response = await _orderService.CountTotalIncome();
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("TotalIncomeUpToDate")]
        public async Task<IActionResult> CountTotalIncomeUpToDate([FromQuery] DateTime to)
        {
            var response = await _orderService.CountTotalIncomeUpToDate(to);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("TotalIncomeByMonth")]
        public async Task<IActionResult> CountTotalIncomeByMonth([FromQuery] int year)
        {
            var response = await _orderService.CountTotalIncomeByMonth(year);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        } 
        
        [HttpGet("GetOrderSuccessByAccountIdAndCourseId")]
        public async Task<IActionResult> GetOrderSuccessByAccountIdAndCourseId([FromQuery]string accountId, int courseId)
        {
            var response = await _orderService.GetOrderSuccessByAccountIdAndCourseId(accountId, courseId);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        
        [HttpGet("GetOrderPendingByAccountIdAndCourseId")]
        public async Task<IActionResult> GetOrderPendingByAccountIdAndCourseId([FromQuery]string accountId, int courseId)
        {
            var response = await _orderService.GetOrderPendingByAccountIdAndCourseId(accountId, courseId);
            if (response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetClientId")]
        public IActionResult GetClientId()
        {
            var result = _paypalClient.ClientId;
            if (result == null)
            {
                return NotFound();
            }

            var jsonResult = JsonConvert.SerializeObject(result);
            return Ok(jsonResult);
        }
        
        [HttpGet("GetOrderByTransactionId")]
        public async Task<IActionResult> GetOrderByTransactionId(string transactionId)
        {
            var result = await _orderService.GetOrderByTransactionId(transactionId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("AddOrderToCartWaitingPAYMENT")]
        public async Task<IActionResult> AddOrderToDB(AddOrderPaymentModel addOrderPaymentModel)
        {
            var result = await _orderService.AddCourseToPayment(addOrderPaymentModel);
            if (result.Status == "Error")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateOrderWithPaypal")]
        public async Task<IActionResult> CreateOrderPaypal(string accountId, int courseId)
        {
            var result = await _orderService.CreatePaymentWithPayPal(accountId, courseId);
            if (result.Status == "Error")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
        [HttpPost("CreateCapturetWithPayPal")]
        public async Task<IActionResult> CreateCapturetWithPayPal(string transactionId)
        {
            var result = await _orderService.CreateCapturetWithPayPal(transactionId);
            if (result.Status == "Error")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetOrderWithFilter")]
        [Authorize]
        public async Task<IActionResult> GetOrderWithFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] OrderFilterParameter orderFilterParameter)
        {
            try
            {
                var response = await _orderService.GetOrderWithFilter(paginationParameter, orderFilterParameter);
                var metadata = new
                {
                    response.TotalCount,
                    response.PageSize,
                    response.CurrentPage,
                    response.TotalPages,
                    response.HasNext,
                    response.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                if (!response.Any())
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
