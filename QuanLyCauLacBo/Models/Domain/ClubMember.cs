using System.ComponentModel.DataAnnotations;

namespace QuanLyCauLacBo.Models.Domain
{
    public class ClubMember
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        public int UserId { get; set; }

        // "President", "Vice President", "Secretary", "Treasurer", "Member"
        [MaxLength(50)]
        public string Role { get; set; } = "Member";

        public DateTime JoinedDate { get; set; } = DateTime.Now;

        // "Active", "Inactive", "Pending", "Rejected"
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation
        public Club Club { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
