using System.ComponentModel.DataAnnotations;

namespace QuanLyCauLacBo.Models.Domain
{
    public class Club
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? LogoUrl { get; set; }

        public DateTime FoundedDate { get; set; }

        [MaxLength(200)]
        public string? ContactEmail { get; set; }

        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        // "Active", "Inactive", "Suspended"
        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        [MaxLength(100)]
        public string? Faculty { get; set; }

        public int? AdvisorId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<ClubMember> Members { get; set; } = new List<ClubMember>();
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
