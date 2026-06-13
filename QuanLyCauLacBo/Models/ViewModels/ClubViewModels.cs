using System.ComponentModel.DataAnnotations;
using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Models.ViewModels
{
    public class ClubListViewModel
    {
        public List<Club> Clubs { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public int TotalClubs { get; set; }
    }

    public class ClubDetailViewModel
    {
        public Club Club { get; set; } = null!;
        public List<ClubMember> Members { get; set; } = new();
        public List<Event> UpcomingEvents { get; set; } = new();
        public List<Activity> RecentActivities { get; set; } = new();
        public bool IsCurrentUserMember { get; set; }
        public bool IsCurrentUserManager { get; set; }
        public ClubMember? CurrentUserMembership { get; set; }
    }

    public class ClubFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên câu lạc bộ")]
        [MaxLength(150)]
        [Display(Name = "Tên câu lạc bộ")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày thành lập")]
        public DateTime FoundedDate { get; set; } = DateTime.Now;

        [EmailAddress]
        [Display(Name = "Email liên hệ")]
        public string? ContactEmail { get; set; }

        [Display(Name = "Số điện thoại liên hệ")]
        public string? ContactPhone { get; set; }

        [Display(Name = "Khoa/Bộ môn")]
        public string? Faculty { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Active";

        [Display(Name = "Logo URL")]
        public string? LogoUrl { get; set; }
    }

    public class MemberManagementViewModel
    {
        public Club Club { get; set; } = null!;
        public List<ClubMember> PendingMembers { get; set; } = new();
        public List<ClubMember> ActiveMembers { get; set; } = new();
    }
}
