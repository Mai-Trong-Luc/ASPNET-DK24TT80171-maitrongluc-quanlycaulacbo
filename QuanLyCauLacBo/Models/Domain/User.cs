using System.ComponentModel.DataAnnotations;

namespace QuanLyCauLacBo.Models.Domain
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? StudentId { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        // "Admin", "Student", "ClubManager"
        [Required, MaxLength(20)]
        public string Role { get; set; } = "Student";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<ClubMember> ClubMemberships { get; set; } = new List<ClubMember>();
        public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
        public ICollection<Notification> CreatedNotifications { get; set; } = new List<Notification>();
        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    }
}
