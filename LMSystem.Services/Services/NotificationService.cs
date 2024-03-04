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
        private readonly IAccountRepository _accountRepository;

        public NotificationService(INotificationRepository notificationRepository, IAccountRepository accountRepository)
        {
            _notificationRepository = notificationRepository;
            _accountRepository = accountRepository;
        }

        public async Task<int> AddNotificationByAccountId(string accountId, Notification notification)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return 0;
            }
            var account = await _accountRepository.GetAccountById(accountId);
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
            var account = await _accountRepository.GetAccountById(accountId);
            if(account != null)
            {
                var getAll = await _notificationRepository.GetAllNotificationsByAccountIdAsync(paginationParameter, accountId);
            }
            return null;
        }

        public Task<NotificationModel> GetNotificationById(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNumbersOfUnReadNotification(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<int> MarkAllNotificationByAccountIdIsRead(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<int> MarkNotificationIsReadById(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
