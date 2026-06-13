using System.ComponentModel.DataAnnotations;
using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Models.ViewModels
{
    public class EventListViewModel
    {
        public List<Event> Events { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public int? ClubIdFilter { get; set; }
        public List<Club> Clubs { get; set; } = new();
    }

    public class EventDetailViewModel
    {
        public Event Event { get; set; } = null!;
        public List<EventRegistration> Registrations { get; set; } = new();
        public bool IsCurrentUserRegistered { get; set; }
        public EventRegistration? CurrentUserRegistration { get; set; }
        public int RegisteredCount { get; set; }
        public bool CanRegister { get; set; }
    }

    public class EventFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn câu lạc bộ")]
        [Display(Name = "Câu lạc bộ")]
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sự kiện")]
        [MaxLength(200)]
        [Display(Name = "Tên sự kiện")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(7);

        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7).AddHours(2);

        [MaxLength(200)]
        [Display(Name = "Địa điểm")]
        public string? Location { get; set; }

        [Display(Name = "Số lượng tối đa")]
        public int? MaxParticipants { get; set; }

        [Display(Name = "Hình ảnh URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Upcoming";

        public List<Club> Clubs { get; set; } = new();
    }
}
