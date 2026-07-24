using EduNexus.Areas.Admin.ViewModels.RankingConfiguration;
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
    public class RankingConfigurationController : Controller
    {
        private readonly IRankingConfigurationService
            _rankingConfigurationService;

        public RankingConfigurationController(
            IRankingConfigurationService
                rankingConfigurationService)
        {
            _rankingConfigurationService =
                rankingConfigurationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            RankingConfigurationIndexViewModel viewModel =
                await _rankingConfigurationService
                    .GetIndexAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(
    [Bind(Prefix = "Form")]
    RankingConfigurationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                RankingConfigurationIndexViewModel viewModel =
                    await _rankingConfigurationService
                        .GetIndexAsync();

                viewModel.Form = model;

                return View(
                    nameof(Index),
                    viewModel);
            }

            long currentAdminUserId =
                User.GetCurrentUserId();

            ServiceResult result =
                await _rankingConfigurationService
                    .SaveConfigurationAsync(
                        model,
                        currentAdminUserId);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                RankingConfigurationIndexViewModel viewModel =
                    await _rankingConfigurationService
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