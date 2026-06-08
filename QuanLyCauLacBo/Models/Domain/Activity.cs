using System.ComponentModel.DataAnnotations;

namespace QuanLyCauLacBo.Models.Domain
{
    public class Activity
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        // "Planning", "InProgress", "Completed", "Cancelled"
        [MaxLength(20)]
        public string Status { get; set; } = "Planning";

        public int? Progress { get; set; } // 0-100%

        [MaxLength(500)]
        public string? Result { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Club Club { get; set; } = null!;
        public User CreatedBy { get; set; } = null!;
    }
}
