using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalClubs { get; set; }
        public int TotalEvents { get; set; }
        public int TotalActivities { get; set; }
        public int PendingMemberRequests { get; set; }
        public int UpcomingEvents { get; set; }
        public List<Club> RecentClubs { get; set; } = new();
        public List<Event> UpcomingEventsList { get; set; } = new();
        public List<User> RecentUsers { get; set; } = new();
    }

    public class StudentDashboardViewModel
    {
        public User CurrentUser { get; set; } = null!;
        public List<ClubMember> MyClubs { get; set; } = new();
        public List<EventRegistration> MyRegistrations { get; set; } = new();
        public List<Event> SuggestedEvents { get; set; } = new();
        public List<Notification> RecentNotifications { get; set; } = new();
        public int UnreadNotifications { get; set; }
    }

    public class NotificationFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? ClubId { get; set; }
        public string Type { get; set; } = "Info";
        public bool IsGlobal { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public List<Club> Clubs { get; set; } = new();
    }
}
