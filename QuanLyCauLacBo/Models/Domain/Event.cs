using System.ComponentModel.DataAnnotations;

namespace QuanLyCauLacBo.Models.Domain
{
    public class Event
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public int? MaxParticipants { get; set; }

        public string? ImageUrl { get; set; }

        // "Upcoming", "Ongoing", "Completed", "Cancelled"
        [MaxLength(20)]
        public string Status { get; set; } = "Upcoming";

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Club Club { get; set; } = null!;
        public User CreatedBy { get; set; } = null!;
        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    }
}
