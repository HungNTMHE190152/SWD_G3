using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduNexus.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            string role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            return RedirectByRole(role);
        }

        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.ValidateLoginAsync(model.Username, model.Password);

        if (result == null)
        {
            model.ErrorMessage = "Invalid username or password.";
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new Claim(ClaimTypes.Name, result.FullName),
            new Claim(ClaimTypes.Email, result.Email),
            new Claim(ClaimTypes.Role, result.RoleName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            properties);

        await _authService.UpdateLastLoginAsync(result.AccountId);

        return RedirectByRole(result.RoleName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectByRole(string role)
    {
        if (role == RoleNames.Student)
        {
            return RedirectToAction("Student", "Dashboard");
        }

        if (role == RoleNames.Sme)
        {
            return RedirectToAction("SmeWorkspace", "Dashboard");
        }

        if (role == RoleNames.Admin)
        {
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            string role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            return RedirectByRole(role);
        }

        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _authService.RegisterStudentAsync(model);

            TempData["SuccessMessage"] = "Register successfully. Please login.";
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            model.ErrorMessage = ex.Message;
            return View(model);
        }
    }
}