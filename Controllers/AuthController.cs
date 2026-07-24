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

        //=========================================
        // LOGIN
        //=========================================

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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            LoginResult result =
                await _authService.ValidateLoginAsync(
                    model.Username,
                    model.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage ?? "Login failed.");

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

            ClaimsIdentity identity =
                new ClaimsIdentity(
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

        //=========================================
        // REGISTER
        //=========================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectByRole(
                    User.FindFirstValue(ClaimTypes.Role));
            }

            return View(new RegisterViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            RegisterResult result =
                await _authService.RegisterAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    "",
                    result.ErrorMessage ?? "Register failed.");

                return View(model);
            }

            TempData["Success"] =
                "Register successfully. Please login.";

            return RedirectToAction(nameof(Login));
        }

        //=========================================
        // FORGOT PASSWORD
        //=========================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool success =
                await _authService.SendOtpAsync(model.Email);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Email does not exist.");

                return View(model);
            }

            return RedirectToAction(
                nameof(VerifyOtp),
                new
                {
                    email = model.Email
                });
        }

        //=========================================
        // VERIFY OTP
        //=========================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult VerifyOtp(string email)
        {
            VerifyOtpViewModel vm = new()
            {
                Email = email
            };

            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(
            VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool valid =
                await _authService.VerifyOtpAsync(
                    model.Email,
                    model.Otp);

            if (!valid)
            {
                ModelState.AddModelError(
                    "",
                    "OTP is invalid.");

                return View(model);
            }

            return RedirectToAction(
                nameof(ResetPassword),
                new
                {
                    email = model.Email,
                    otp = model.Otp
                });
        }

        //=========================================
        // RESET PASSWORD
        //=========================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(
            string email,
            string otp)
        {
            ResetPasswordViewModel vm = new()
            {
                Email = email,
                Otp = otp
            };

            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool success =
                await _authService.ResetPasswordAsync(
                    model.Email,
                    model.Otp,
                    model.NewPassword);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "OTP expired or invalid.");

                return View(model);
            }

            TempData["Success"] =
                "Password has been changed successfully.";

            return RedirectToAction(nameof(Login));
        }

        //=========================================
        // LOGOUT
        //=========================================

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        //=========================================
        // ACCESS DENIED
        //=========================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //=========================================
        // REDIRECT BY ROLE
        //=========================================

        private IActionResult RedirectByRole(string? roleName)
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

                _ => RedirectToAction(nameof(AccessDenied))
            };
        }
    }
}