using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Models.ViewModels;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IClubService _clubService;

        public EventController(IEventService eventService, IClubService clubService)
        {
            _eventService = eventService;
            _clubService = clubService;
        }

        public async Task<IActionResult> Index(string? search, string? status, int? clubId)
        {
            var events = await _eventService.GetAllEventsAsync(search, status, clubId);
            var clubs = await _clubService.GetAllClubsAsync();
            var vm = new EventListViewModel
            {
                Events = events,
                SearchTerm = search,
                StatusFilter = status,
                ClubIdFilter = clubId,
                Clubs = clubs
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var evt = await _eventService.GetEventByIdAsync(id);
            if (evt == null) return NotFound();

            bool isRegistered = false;
            EventRegistration? myReg = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                myReg = await _eventService.GetRegistrationAsync(id, userId);
                isRegistered = myReg?.Status == "Registered" || myReg?.Status == "Attended";
            }

            var registeredCount = evt.Registrations.Count(r => r.Status != "Cancelled");
            var canRegister = evt.Status == "Upcoming"
                && (!evt.MaxParticipants.HasValue || registeredCount < evt.MaxParticipants.Value)
                && User.Identity?.IsAuthenticated == true
                && !isRegistered;

            var vm = new EventDetailViewModel
            {
                Event = evt,
                Registrations = evt.Registrations.ToList(),
                IsCurrentUserRegistered = isRegistered,
                CurrentUserRegistration = myReg,
                RegisteredCount = registeredCount,
                CanRegister = canRegister
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public async Task<IActionResult> Create()
        {
            var clubs = await _clubService.GetAllClubsAsync(status: "Active");
            var vm = new EventFormViewModel
            {
                Clubs = clubs,
                StartDate = DateTime.Now.AddDays(7),
                EndDate = DateTime.Now.AddDays(7).AddHours(2)
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (model.EndDate <= model.StartDate)
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải sau ngày bắt đầu.");

            if (!ModelState.IsValid)
            {
                model.Clubs = await _clubService.GetAllClubsAsync(status: "Active");
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var evt = new Event
            {
                ClubId = model.ClubId,
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Location = model.Location,
                MaxParticipants = model.MaxParticipants,
                ImageUrl = model.ImageUrl,
                Status = "Upcoming",
                CreatedById = userId
            };

            var created = await _eventService.CreateEventAsync(evt);
            TempData["Success"] = $"Sự kiện '{created.Title}' đã được tạo!";
            return RedirectToAction("Details", new { id = created.Id });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public async Task<IActionResult> Edit(int id)
        {
            var evt = await _eventService.GetEventByIdAsync(id);
            if (evt == null) return NotFound();

            var clubs = await _clubService.GetAllClubsAsync(status: "Active");
            var vm = new EventFormViewModel
            {
                Id = evt.Id,
                ClubId = evt.ClubId,
                Title = evt.Title,
                Description = evt.Description,
                StartDate = evt.StartDate,
                EndDate = evt.EndDate,
                Location = evt.Location,
                MaxParticipants = evt.MaxParticipants,
                ImageUrl = evt.ImageUrl,
                Status = evt.Status,
                Clubs = clubs
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventFormViewModel model)
        {
            if (model.EndDate <= model.StartDate)
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải sau ngày bắt đầu.");

            if (!ModelState.IsValid)
            {
                model.Clubs = await _clubService.GetAllClubsAsync(status: "Active");
                return View(model);
            }

            var evt = await _eventService.GetEventByIdAsync(id);
            if (evt == null) return NotFound();

            evt.ClubId = model.ClubId;
            evt.Title = model.Title;
            evt.Description = model.Description;
            evt.StartDate = model.StartDate;
            evt.EndDate = model.EndDate;
            evt.Location = model.Location;
            evt.MaxParticipants = model.MaxParticipants;
            evt.ImageUrl = model.ImageUrl;
            evt.Status = model.Status;

            await _eventService.UpdateEventAsync(evt);
            TempData["Success"] = "Cập nhật sự kiện thành công!";
            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventService.DeleteEventAsync(id);
            TempData["Success"] = "Đã xóa sự kiện!";
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _eventService.RegisterEventAsync(id, userId);

            TempData[result ? "Success" : "Error"] = result
                ? "Đăng ký tham gia sự kiện thành công!"
                : "Không thể đăng ký. Sự kiện đã đủ chỗ hoặc bạn đã đăng ký rồi.";

            return RedirectToAction("Details", new { id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRegistration(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _eventService.CancelRegistrationAsync(id, userId);
            TempData["Success"] = "Đã hủy đăng ký sự kiện.";
            return RedirectToAction("Details", new { id });
        }
    }
}
