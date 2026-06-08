using System.ComponentModel.DataAnnotations;

namespace QuanLyCauLacBo.Models.Domain
{
    public class Notification
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string Content { get; set; } = string.Empty;

        // null = global (toàn trường)
        public int? ClubId { get; set; }

        public int CreatedById { get; set; }

        // "Info", "Warning", "Event", "Urgent"
        [MaxLength(20)]
        public string Type { get; set; } = "Info";

        public bool IsGlobal { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiresAt { get; set; }

        // Navigation
        public Club? Club { get; set; }
        public User CreatedBy { get; set; } = null!;
        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    }
}
