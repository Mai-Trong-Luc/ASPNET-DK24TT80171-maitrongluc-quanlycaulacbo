using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Models.ViewModels;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IClubService _clubService;

        public ActivityController(IActivityService activityService, IClubService clubService)
        {
            _activityService = activityService;
            _clubService = clubService;
        }

        public async Task<IActionResult> Index(string? search, string? status, int? clubId)
        {
            var activities = await _activityService.GetAllActivitiesAsync(search, status, clubId);
            var clubs = await _clubService.GetAllClubsAsync();
            var vm = new ActivityListViewModel
            {
                Activities = activities,
                SearchTerm = search,
                StatusFilter = status,
                ClubIdFilter = clubId,
                Clubs = clubs
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null) return NotFound();
            return View(new ActivityDetailViewModel { Activity = activity });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public async Task<IActionResult> Create()
        {
            var clubs = await _clubService.GetAllClubsAsync(status: "Active");
            return View(new ActivityFormViewModel { Clubs = clubs, StartDate = DateTime.Now });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clubs = await _clubService.GetAllClubsAsync(status: "Active");
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var activity = new Activity
            {
                ClubId = model.ClubId,
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Location = model.Location,
                Status = model.Status,
                Progress = model.Progress ?? 0,
                Result = model.Result,
                CreatedById = userId
            };

            var created = await _activityService.CreateActivityAsync(activity);
            TempData["Success"] = $"Hoạt động '{created.Title}' đã được tạo!";
            return RedirectToAction("Details", new { id = created.Id });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public async Task<IActionResult> Edit(int id)
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null) return NotFound();

            var clubs = await _clubService.GetAllClubsAsync(status: "Active");
            var vm = new ActivityFormViewModel
            {
                Id = activity.Id,
                ClubId = activity.ClubId,
                Title = activity.Title,
                Description = activity.Description,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                Location = activity.Location,
                Status = activity.Status,
                Progress = activity.Progress,
                Result = activity.Result,
                Clubs = clubs
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clubs = await _clubService.GetAllClubsAsync(status: "Active");
                return View(model);
            }

            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null) return NotFound();

            activity.ClubId = model.ClubId;
            activity.Title = model.Title;
            activity.Description = model.Description;
            activity.StartDate = model.StartDate;
            activity.EndDate = model.EndDate;
            activity.Location = model.Location;
            activity.Status = model.Status;
            activity.Progress = model.Progress ?? 0;
            activity.Result = model.Result;

            await _activityService.UpdateActivityAsync(activity);
            TempData["Success"] = "Cập nhật hoạt động thành công!";
            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _activityService.DeleteActivityAsync(id);
            TempData["Success"] = "Đã xóa hoạt động!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProgress(int id, int progress, string? result)
        {
            await _activityService.UpdateProgressAsync(id, progress, result);
            TempData["Success"] = $"Đã cập nhật tiến độ: {progress}%";
            return RedirectToAction("Details", new { id });
        }
    }
}
