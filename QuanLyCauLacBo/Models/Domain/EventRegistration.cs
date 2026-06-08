using System.ComponentModel.DataAnnotations;

namespace QuanLyCauLacBo.Models.Domain
{
    public class EventRegistration
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int UserId { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        // "Registered", "Attended", "Absent", "Cancelled"
        [MaxLength(20)]
        public string Status { get; set; } = "Registered";

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation
        public Event Event { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
