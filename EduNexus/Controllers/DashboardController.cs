using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> Student()
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _dashboardService.GetStudentDashboardAsync(studentId);
        return View(model);
    }

    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> PersonalProgress()
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _dashboardService.GetPersonalProgressAsync(studentId);
        return View(model);
    }

    [Authorize(Roles = RoleNames.Sme)]
    public IActionResult SmeWorkspace()
    {
        return View();
    }
}