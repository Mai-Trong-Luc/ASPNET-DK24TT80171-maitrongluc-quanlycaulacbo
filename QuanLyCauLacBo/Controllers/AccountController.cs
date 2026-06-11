using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Models.ViewModels;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public AccountController(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = model.RememberMe });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _userService.UsernameExistsAsync(model.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                return View(model);
            }

            if (await _userService.EmailExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                StudentId = model.StudentId,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                Role = "Student",
                IsActive = true
            };

            await _userService.RegisterAsync(user, model.Password);

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            var vm = new ProfileViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                StudentId = user.StudentId,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.DateOfBirth = model.DateOfBirth;

            await _userService.UpdateProfileAsync(user);

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profile");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword() => View();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _userService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (!result)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return View(model);
            }

            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Profile");
        }

        public IActionResult AccessDenied() => View();
    }
}
