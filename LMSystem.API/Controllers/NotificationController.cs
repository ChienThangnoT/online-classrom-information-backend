using FirebaseAdmin.Messaging;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Models;
using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMSystem.API.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<ActionResult> GetAllNotificationsByAccountIdAsync(PaginationParameter paginationParameter, string accountId)
        {
            try
            {
                var getAllNoti = await _notificationService.GetAllNotificationsByAccountIdAsync(paginationParameter, accountId);
                if (getAllNoti == null)
                {
                    return NotFound();
                }
                var metadata = new
                {
                    getAllNoti.TotalCount,
                    getAllNoti.PageSize,
                    getAllNoti.CurrentPage,
                    getAllNoti.TotalPages,
                    getAllNoti.HasNext,
                    getAllNoti.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(getAllNoti);
            }catch
            {
                return BadRequest();
            }
        }
    }
}
