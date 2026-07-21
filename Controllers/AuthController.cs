using System.Security.Claims;
using EduNexus.Constants;
using EduNexus.Services.Authentication.Interfaces;
using EduNexus.ViewModels.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectByRole(
                    User.FindFirstValue(ClaimTypes.Role));
            }

            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            LoginResult result =
                await _authService.ValidateLoginAsync(
                    model.Username,
                    model.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage
                    ?? "Login failed.");

                return View(model);
            }

            List<Claim> claims =
            [
                new Claim(
                    ClaimTypes.NameIdentifier,
                    result.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    result.FullName),

                new Claim(
                    ClaimTypes.Email,
                    result.Email),

                new Claim(
                    ClaimTypes.Role,
                    result.RoleName),

                new Claim(
                    "AccountId",
                    result.AccountId.ToString())
            ];

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal =
                new ClaimsPrincipal(identity);

            AuthenticationProperties properties =
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    AllowRefresh = true
                };

            if (model.RememberMe)
            {
                properties.ExpiresUtc =
                    DateTimeOffset.UtcNow.AddDays(7);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);

            return RedirectByRole(result.RoleName);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectByRole(
            string? roleName)
        {
            return roleName switch
            {
                RoleNames.Admin => RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Admin" }),

                RoleNames.Sme => RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "SME" }),

                RoleNames.Teacher => RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Teacher" }),

                RoleNames.Student => RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Student" }),

                _ => RedirectToAction(
                    nameof(AccessDenied))
            };
        }
    }
}