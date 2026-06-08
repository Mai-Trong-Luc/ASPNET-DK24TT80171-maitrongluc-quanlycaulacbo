namespace QuanLyCauLacBo.Models.Domain
{
    public class UserNotification
    {
        public int Id { get; set; }

        public int NotificationId { get; set; }

        public int UserId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        // Navigation
        public Notification Notification { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
