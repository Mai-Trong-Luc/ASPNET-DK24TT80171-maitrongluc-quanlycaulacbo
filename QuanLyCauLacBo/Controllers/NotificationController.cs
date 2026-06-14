using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Models.ViewModels;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IClubService _clubService;

        public NotificationController(INotificationService notificationService, IClubService clubService)
        {
            _notificationService = notificationService;
            _clubService = clubService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var notifications = await _notificationService.GetNotificationsForUserAsync(userId);
            ViewBag.UnreadCount = await _notificationService.GetUnreadCountAsync(userId);
            return View(notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkRead(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _notificationService.MarkAsReadAsync(id, userId);
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllRead()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _notificationService.MarkAllAsReadAsync(userId);
            TempData["Success"] = "Đã đánh dấu tất cả là đã đọc.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public async Task<IActionResult> Create()
        {
            var clubs = await _clubService.GetAllClubsAsync(status: "Active");
            var vm = new NotificationFormViewModel { Clubs = clubs };
            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clubs = await _clubService.GetAllClubsAsync(status: "Active");
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var notification = new Notification
            {
                Title = model.Title,
                Content = model.Content,
                ClubId = model.IsGlobal ? null : model.ClubId,
                Type = model.Type,
                IsGlobal = model.IsGlobal,
                CreatedById = userId,
                ExpiresAt = model.ExpiresAt
            };

            await _notificationService.CreateNotificationAsync(notification);
            TempData["Success"] = "Thông báo đã được gửi!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            TempData["Success"] = "Đã xóa thông báo.";
            return RedirectToAction("Index");
        }
    }
}
