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
    }
}
