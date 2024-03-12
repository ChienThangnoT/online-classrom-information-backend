using AutoMapper;
using AutoMapper.QueryableExtensions;
using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly LMOnlineSystemDbContext _context;
        private readonly IMapper _mapper;
        public NotificationRepository(LMOnlineSystemDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddNotificationByAccountId(string accountId, Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification.NotificationId;
        }

        public async Task<PagedList<NotificationModel>> GetAllNotificationsByAccountIdAsync(PaginationParameter paginationParameter, string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return null;
            }
            var notifications = _context.Notifications.Where(x => x.AccountId == accountId).AsQueryable();
            if (notifications.Any())
            {
                var allNoti = await notifications.OrderByDescending(x => x.SendDate)
                    .ProjectTo<NotificationModel>(_mapper.ConfigurationProvider).ToListAsync();

                return PagedList<NotificationModel>.ToPagedList(allNoti,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
            }
            return null;
        }

        public async Task<NotificationModel> GetNotificationById(int notificationId)
        {
            if (notificationId <= 0)
            {
                return null;
            }
            var notification = await _context.Notifications.SingleOrDefaultAsync(x => x.NotificationId == notificationId);
            return _mapper.Map<NotificationModel>(notification);
        }

        public async Task<int> GetNumbersOfUnReadNotification(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return 0;
            }
            var numbers = await _context.Notifications.Where(x => x.AccountId == accountId && x.IsRead == false).CountAsync();
            return numbers;
        }

        public async Task<int> MarkAllNotificationByAccountIdIsRead(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return 0;
            }
            var notifications = await _context.Notifications.Where(x => x.AccountId == accountId).ToListAsync();
            foreach (var noti in notifications)
            {
                if (noti.IsRead == false)
                {
                    noti.IsRead = true;
                }
            }
            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> MarkNotificationIsReadById(int notificationId)
        {
            if (notificationId <= 0)
            {
                return 0;
            }
            var notification = await _context.Notifications.SingleOrDefaultAsync(x => x.NotificationId == notificationId);
            if (notification != null)
            {
                if (notification.IsRead == false)
                {
                    notification.IsRead = true;
                }
                _context.Update(notification);
                await _context.SaveChangesAsync();
                return notification.NotificationId;
            }
            return 0;
        }
    }
}
