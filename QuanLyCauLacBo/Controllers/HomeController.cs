using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using QuanLyCauLacBo.Models.ViewModels;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClubService _clubService;
        private readonly IEventService _eventService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public HomeController(IClubService clubService, IEventService eventService,
            INotificationService notificationService, IUserService userService)
        {
            _clubService = clubService;
            _eventService = eventService;
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var role = User.FindFirstValue(ClaimTypes.Role);

                if (role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");

                var user = await _userService.GetUserByIdAsync(userId);
                var events = await _eventService.GetAllEventsAsync(status: "Upcoming");
                var notifications = await _notificationService.GetNotificationsForUserAsync(userId);
                var unreadCount = await _notificationService.GetUnreadCountAsync(userId);

                var vm = new StudentDashboardViewModel
                {
                    CurrentUser = user!,
                    SuggestedEvents = events.Take(5).ToList(),
                    RecentNotifications = notifications.Take(5).ToList(),
                    UnreadNotifications = unreadCount
                };

                return View("StudentDashboard", vm);
            }

            // Trang chủ cho khách
            var allClubs = await _clubService.GetAllClubsAsync(status: "Active");
            var upcomingEvents = await _eventService.GetAllEventsAsync(status: "Upcoming");

            ViewBag.Clubs = allClubs.Take(6).ToList();
            ViewBag.Events = upcomingEvents.Take(4).ToList();
            return View();
        }

        public IActionResult About() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
