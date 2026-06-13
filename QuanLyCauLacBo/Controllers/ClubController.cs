using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Models.ViewModels;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;
        private readonly IEventService _eventService;
        private readonly IActivityService _activityService;

        public ClubController(IClubService clubService, IEventService eventService, IActivityService activityService)
        {
            _clubService = clubService;
            _eventService = eventService;
            _activityService = activityService;
        }

        public async Task<IActionResult> Index(string? search, string? status)
        {
            var clubs = await _clubService.GetAllClubsAsync(search, status);
            var vm = new ClubListViewModel
            {
                Clubs = clubs,
                SearchTerm = search,
                StatusFilter = status,
                TotalClubs = clubs.Count
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var club = await _clubService.GetClubByIdAsync(id);
            if (club == null) return NotFound();

            var upcomingEvents = await _eventService.GetAllEventsAsync(status: "Upcoming", clubId: id);
            var recentActivities = await _activityService.GetAllActivitiesAsync(clubId: id);

            bool isMember = false, isManager = false;
            ClubMember? currentMembership = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                currentMembership = await _clubService.GetMembershipAsync(id, userId);
                isMember = currentMembership?.Status == "Active";
                isManager = isMember && (currentMembership?.Role == "President" || currentMembership?.Role == "Vice President")
                            || User.IsInRole("Admin");
            }

            var vm = new ClubDetailViewModel
            {
                Club = club,
                Members = club.Members.Where(m => m.Status == "Active").ToList(),
                UpcomingEvents = upcomingEvents.Take(5).ToList(),
                RecentActivities = recentActivities.Take(5).ToList(),
                IsCurrentUserMember = isMember,
                IsCurrentUserManager = isManager,
                CurrentUserMembership = currentMembership
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public IActionResult Create() => View(new ClubFormViewModel { FoundedDate = DateTime.Now });

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClubFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var club = new Club
            {
                Name = model.Name,
                Description = model.Description,
                FoundedDate = model.FoundedDate,
                ContactEmail = model.ContactEmail,
                ContactPhone = model.ContactPhone,
                Faculty = model.Faculty,
                Status = model.Status,
                LogoUrl = model.LogoUrl
            };

            var created = await _clubService.CreateClubAsync(club);
            TempData["Success"] = $"Câu lạc bộ '{created.Name}' đã được tạo thành công!";
            return RedirectToAction("Details", new { id = created.Id });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubService.GetClubByIdAsync(id);
            if (club == null) return NotFound();

            var vm = new ClubFormViewModel
            {
                Id = club.Id,
                Name = club.Name,
                Description = club.Description,
                FoundedDate = club.FoundedDate,
                ContactEmail = club.ContactEmail,
                ContactPhone = club.ContactPhone,
                Faculty = club.Faculty,
                Status = club.Status,
                LogoUrl = club.LogoUrl
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClubFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var club = await _clubService.GetClubByIdAsync(id);
            if (club == null) return NotFound();

            club.Name = model.Name;
            club.Description = model.Description;
            club.FoundedDate = model.FoundedDate;
            club.ContactEmail = model.ContactEmail;
            club.ContactPhone = model.ContactPhone;
            club.Faculty = model.Faculty;
            club.Status = model.Status;
            club.LogoUrl = model.LogoUrl;

            await _clubService.UpdateClubAsync(club);
            TempData["Success"] = "Cập nhật thông tin câu lạc bộ thành công!";
            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _clubService.DeleteClubAsync(id);
            TempData["Success"] = "Đã xóa câu lạc bộ!";
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _clubService.JoinClubAsync(id, userId);

            TempData[result ? "Success" : "Error"] = result
                ? "Đăng ký tham gia câu lạc bộ thành công! Chờ ban quản lý xét duyệt."
                : "Bạn đã đăng ký câu lạc bộ này rồi.";

            return RedirectToAction("Details", new { id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _clubService.LeaveClubAsync(id, userId);
            TempData["Success"] = "Bạn đã rời câu lạc bộ.";
            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        public async Task<IActionResult> ManageMembers(int id)
        {
            var club = await _clubService.GetClubByIdAsync(id);
            if (club == null) return NotFound();

            var allMembers = await _clubService.GetClubMembersAsync(id);
            var vm = new MemberManagementViewModel
            {
                Club = club,
                PendingMembers = allMembers.Where(m => m.Status == "Pending").ToList(),
                ActiveMembers = allMembers.Where(m => m.Status == "Active").ToList()
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveMember(int clubMemberId, int clubId)
        {
            await _clubService.ApproveMemberAsync(clubMemberId);
            TempData["Success"] = "Đã duyệt thành viên!";
            return RedirectToAction("ManageMembers", new { id = clubId });
        }

        [Authorize(Roles = "Admin,ClubManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectMember(int clubMemberId, int clubId)
        {
            await _clubService.RejectMemberAsync(clubMemberId);
            TempData["Error"] = "Đã từ chối thành viên.";
            return RedirectToAction("ManageMembers", new { id = clubId });
        }
    }
}
