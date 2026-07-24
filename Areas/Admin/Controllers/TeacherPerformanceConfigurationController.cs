using EduNexus.Areas.Admin.ViewModels
    .TeacherPerformanceConfiguration;
using EduNexus.Constants;
using EduNexus.Extensions;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class TeacherPerformanceConfigurationController
        : Controller
    {
        private readonly
            ITeacherPerformanceConfigurationService
            _configurationService;

        public TeacherPerformanceConfigurationController(
            ITeacherPerformanceConfigurationService
                configurationService)
        {
            _configurationService =
                configurationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            TeacherPerformanceConfigurationIndexViewModel
                viewModel =
                    await _configurationService
                        .GetIndexAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(
            [Bind(Prefix = "Form")]
            TeacherPerformanceConfigurationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TeacherPerformanceConfigurationIndexViewModel
                    viewModel =
                        await _configurationService
                            .GetIndexAsync();

                viewModel.Form = model;

                return View(
                    nameof(Index),
                    viewModel);
            }

            long currentAdminUserId =
                User.GetCurrentUserId();

            ServiceResult result =
                await _configurationService
                    .SaveConfigurationAsync(
                        model,
                        currentAdminUserId);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                TeacherPerformanceConfigurationIndexViewModel
                    viewModel =
                        await _configurationService
                            .GetIndexAsync();

                viewModel.Form = model;

                return View(
                    nameof(Index),
                    viewModel);
            }

            TempData["SuccessMessage"] =
                result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}