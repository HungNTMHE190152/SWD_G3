using EduNexus.Areas.Admin.ViewModels.Dashboard;
using EduNexus.Constants;
using EduNexus.Services.Administration.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class DashboardController : Controller
    {
        private readonly IAdminDashboardService
            _adminDashboardService;

        public DashboardController(
            IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AdminDashboardViewModel viewModel =
                await _adminDashboardService
                    .GetDashboardAsync();

            return View(viewModel);
        }
    }
}