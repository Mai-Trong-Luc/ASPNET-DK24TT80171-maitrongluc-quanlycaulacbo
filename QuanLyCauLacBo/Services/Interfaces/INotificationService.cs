using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsForUserAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
        Task<Notification> CreateNotificationAsync(Notification notification, List<int>? targetUserIds = null);
        Task DeleteNotificationAsync(int id);
        Task<List<Notification>> GetAllNotificationsAsync();
    }
}
