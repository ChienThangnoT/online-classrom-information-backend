using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAccountService _accountService;

        public NotificationService(INotificationRepository notificationRepository, IAccountService accountService)
        {
            _notificationRepository = notificationRepository;
            _accountService = accountService;
        }

        public async Task<int> AddNotificationByAccountId(string accountId, Notification notification)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return 0;
            }
            var account = await _accountService.GetAccountById(accountId);
            if (account != null && notification != null)
            {
                var addNoti = _notificationRepository.AddNotificationByAccountId(accountId, notification);
                return addNoti.Id;
            }
            return 0;
        }

        public async Task<PagedList<NotificationModel>> GetAllNotificationsByAccountIdAsync(PaginationParameter paginationParameter, string accountId)
        {
            if(string.IsNullOrEmpty(accountId))
            {
                return null;
            }
            var account = await _accountService.GetAccountById(accountId);
            if(account != null)
            {
                return await _notificationRepository.GetAllNotificationsByAccountIdAsync(paginationParameter, accountId);
            }
            return null;
        }

        public async Task<NotificationModel> GetNotificationById(int notificationId)
        {
            if(notificationId == 0)
            {
                return null;
            }
            var notification = await _notificationRepository.GetNotificationById(notificationId);
            if (notification != null)
            {
                return notification;
            }
            return null;
        }

        public async Task<int> GetNumbersOfUnReadNotification(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return -1;
            }
            var account = await _accountService.GetAccountById(accountId);
            if (account != null)
            {
                return await _notificationRepository.GetNumbersOfUnReadNotification(account.Id);
            }
            return -1;
        }

        public async Task<int> MarkAllNotificationByAccountIdIsRead(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return -1;
            }
            var account = await _accountService.GetAccountById(accountId);
            if (account != null)
            {
                return await _notificationRepository.MarkAllNotificationByAccountIdIsRead(account.Id);
            }
            return -1;
        }

        public async Task<int> MarkNotificationIsReadById(int notificationId)
        {
            if (notificationId == 0)
            {
                return -1;
            }
            var notifi = await _notificationRepository.GetNotificationById(notificationId);
            if (notifi != null)
            {
                return await _notificationRepository.MarkNotificationIsReadById(notifi.NotificationId);
            }
            return -1;
        }
    }
}
