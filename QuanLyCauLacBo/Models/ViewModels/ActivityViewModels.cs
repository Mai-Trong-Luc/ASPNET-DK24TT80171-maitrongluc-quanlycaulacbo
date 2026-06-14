using System.ComponentModel.DataAnnotations;
using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Models.ViewModels
{
    public class ActivityListViewModel
    {
        public List<Activity> Activities { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public int? ClubIdFilter { get; set; }
        public List<Club> Clubs { get; set; } = new();
    }

    public class ActivityDetailViewModel
    {
        public Activity Activity { get; set; } = null!;
    }

    public class ActivityFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn câu lạc bộ")]
        [Display(Name = "Câu lạc bộ")]
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên hoạt động")]
        [MaxLength(200)]
        [Display(Name = "Tên hoạt động")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        [MaxLength(200)]
        [Display(Name = "Địa điểm")]
        public string? Location { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Planning";

        [Range(0, 100, ErrorMessage = "Tiến độ từ 0 đến 100")]
        [Display(Name = "Tiến độ (%)")]
        public int? Progress { get; set; }

        [MaxLength(500)]
        [Display(Name = "Kết quả")]
        public string? Result { get; set; }

        public List<Club> Clubs { get; set; } = new();
    }
}
