using EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport;
using EduNexus.Constants;
using EduNexus.Services.Administration.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class TeacherPerformanceReportController
        : Controller
    {
        private readonly ITeacherPerformanceReportService
            _reportService;

        public TeacherPerformanceReportController(
            ITeacherPerformanceReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery]
            TeacherPerformanceReportFilterViewModel filter)
        {
            TeacherPerformanceReportIndexViewModel viewModel =
                await _reportService
                    .GetReportAsync(filter);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            TeacherPerformanceDetailsViewModel? viewModel =
                await _reportService
                    .GetDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}