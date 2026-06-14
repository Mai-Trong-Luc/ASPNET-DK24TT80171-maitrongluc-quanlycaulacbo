using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyCauLacBo.Models.ViewModels;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IClubService _clubService;
        private readonly IEventService _eventService;
        private readonly IActivityService _activityService;
        private readonly INotificationService _notificationService;

        public AdminController(IUserService userService, IClubService clubService,
            IEventService eventService, IActivityService activityService,
            INotificationService notificationService)
        {
            _userService = userService;
            _clubService = clubService;
            _eventService = eventService;
            _activityService = activityService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var users = await _userService.GetAllUsersAsync();
            var clubs = await _clubService.GetAllClubsAsync();
            var events = await _eventService.GetAllEventsAsync();
            var activities = await _activityService.GetAllActivitiesAsync();

            // Tính pending member requests
            int pendingCount = 0;
            foreach (var club in clubs)
            {
                var members = await _clubService.GetClubMembersAsync(club.Id);
                pendingCount += members.Count(m => m.Status == "Pending");
            }

            var vm = new AdminDashboardViewModel
            {
                TotalUsers = users.Count,
                TotalClubs = clubs.Count,
                TotalEvents = events.Count,
                TotalActivities = activities.Count,
                PendingMemberRequests = pendingCount,
                UpcomingEvents = events.Count(e => e.Status == "Upcoming"),
                RecentClubs = clubs.OrderByDescending(c => c.CreatedAt).Take(5).ToList(),
                UpcomingEventsList = events.Where(e => e.Status == "Upcoming").Take(5).ToList(),
                RecentUsers = users.OrderByDescending(u => u.CreatedAt).Take(5).ToList()
            };

            return View(vm);
        }

        // Quản lý người dùng
        public async Task<IActionResult> Users(string? search, string? role)
        {
            var users = await _userService.GetAllUsersAsync();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(u => u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || u.Username.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || (u.StudentId != null && u.StudentId.Contains(search))).ToList();

            if (!string.IsNullOrEmpty(role))
                users = users.Where(u => u.Role == role).ToList();

            ViewBag.SearchTerm = search;
            ViewBag.RoleFilter = role;
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            await _userService.SetUserActiveAsync(userId, !user.IsActive);
            TempData["Success"] = user.IsActive ? "Đã khóa tài khoản người dùng." : "Đã kích hoạt tài khoản.";
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(int userId, string role)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            user.Role = role;
            await _userService.UpdateProfileAsync(user);
            TempData["Success"] = $"Đã cập nhật vai trò người dùng thành '{role}'.";
            return RedirectToAction("Users");
        }

        // Quản lý thông báo
        public async Task<IActionResult> Notifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return View(notifications);
        }
    }
}
