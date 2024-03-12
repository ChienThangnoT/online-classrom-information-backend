using FirebaseAdmin.Messaging;
using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Models;
using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpGet("GetAllNotificationsByAccountId")]
        public async Task<ActionResult> GetAllNotificationsByAccountIdAsync([FromQuery]PaginationParameter paginationParameter, string accountId)
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

        [HttpGet("GetNotificationById")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            //try
            //{
                var notification = await _notificationService.GetNotificationById(id);
                if(notification == null)
                {
                    return NotFound();
                }
                return Ok(notification);
            //}
            //catch
            //{
            //    return BadRequest();
            //}
        }

        [HttpGet("MarkAllNotificationByAccountIdIsRead")]
        public async Task<IActionResult> MarkAllNotificationByAccountIdIsRead(string accountId)
        {
            try
            {
                var result = await _notificationService.MarkAllNotificationByAccountIdIsRead(accountId);
                if (result == -1)
                {
                    ResponeModel rss = new ResponeModel
                    {
                        Status = "Error",
                        Message = "Cannot mark notifications is read",
                    };
                    return NotFound(rss);
                }
                ResponeModel rs = new ResponeModel
                {
                    Status = "Success",
                    Message = "Mark all notifications is read successfully",
                };
                return Ok(rs);
            }
            catch
            {
                return BadRequest();
            }
        } 

        [HttpGet("MarkNotificationIsReadByNotiId")]
        public async Task<IActionResult> MarkNotificationIsReadById(int notificationId)
        {
            try
            {
                var result = await _notificationService.MarkNotificationIsReadById(notificationId);
                if (result == -1)
                {
                    ResponeModel rss = new ResponeModel
                    {
                        Status = "Error",
                        Message = "Cannot mark notifications is read",
                    };
                    return NotFound(rss);
                }
                ResponeModel rs = new ResponeModel
                {
                    Status = "Success",
                    Message = "Mark notifications is read successfully",
                };
                return Ok(rs);
            }
            catch
            {
                return BadRequest();
            }
        }
        

        [HttpGet("GetNumbersOfUnReadNotification")]
        public async Task<IActionResult> GetNumbersOfUnReadNotification(string accountId)
        {
            try
            {
                var result = await _notificationService.GetNumbersOfUnReadNotification(accountId);
                if (result == -1)
                {
                    ResponeModel rss = new ResponeModel
                    {
                        Status = "Error",
                        Message = "Cannot get number of unread notifications!",
                    };
                    return NotFound(rss);
                }
                ResponeModel rs = new ResponeModel
                {
                    Status = "Success",
                    Message = "Get number of unread notifications successfully",
                    DataObject = result
                };
                return Ok(rs);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
