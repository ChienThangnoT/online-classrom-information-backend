using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderRepository)
        {
            _orderService = orderRepository;
        }

        [HttpGet("PaymentHistory")]
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
    }
}
