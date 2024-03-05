using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface INotificationRepository
    {
        public Task<PagedList<NotificationModel>> GetAllNotificationsByAccountIdAsync(PaginationParameter paginationParameter, string accountId);

        public Task<NotificationModel> GetNotificationById(int notificationId);

        public Task<int> AddNotificationByAccountId(string accountId, Notification notification);

        public Task<int> MarkAllNotificationByAccountIdIsRead(string accountId);

        public Task<int> MarkNotificationIsReadById(int notificationId);

        public Task<int> GetNumbersOfUnReadNotification(string accountId);
    }
}
