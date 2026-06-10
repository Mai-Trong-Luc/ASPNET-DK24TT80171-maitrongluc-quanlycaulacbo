using Microsoft.EntityFrameworkCore;
using QuanLyCauLacBo.Data;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetNotificationsForUserAsync(int userId)
        {
            var notifIds = await _context.UserNotifications
                .Where(un => un.UserId == userId)
                .Select(un => un.NotificationId)
                .ToListAsync();

            return await _context.Notifications
                .Include(n => n.Club)
                .Where(n => notifIds.Contains(n.Id) && (n.ExpiresAt == null || n.ExpiresAt > DateTime.Now))
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
            => await _context.UserNotifications
                .CountAsync(un => un.UserId == userId && !un.IsRead);

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var un = await _context.UserNotifications
                .FirstOrDefaultAsync(u => u.NotificationId == notificationId && u.UserId == userId);

            if (un != null && !un.IsRead)
            {
                un.IsRead = true;
                un.ReadAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var unread = await _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToListAsync();

            foreach (var un in unread)
            {
                un.IsRead = true;
                un.ReadAt = DateTime.Now;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification, List<int>? targetUserIds = null)
        {
            notification.CreatedAt = DateTime.Now;
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            List<int> recipients;

            if (notification.IsGlobal)
            {
                recipients = await _context.Users.Where(u => u.IsActive).Select(u => u.Id).ToListAsync();
            }
            else if (targetUserIds != null && targetUserIds.Any())
            {
                recipients = targetUserIds;
            }
            else if (notification.ClubId.HasValue)
            {
                recipients = await _context.ClubMembers
                    .Where(cm => cm.ClubId == notification.ClubId && cm.Status == "Active")
                    .Select(cm => cm.UserId)
                    .ToListAsync();
            }
            else
            {
                recipients = new List<int>();
            }

            var userNotifs = recipients.Select(uid => new UserNotification
            {
                NotificationId = notification.Id,
                UserId = uid,
                IsRead = false
            });

            _context.UserNotifications.AddRange(userNotifs);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
            => await _context.Notifications
                .Include(n => n.Club)
                .Include(n => n.CreatedBy)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
    }
}
