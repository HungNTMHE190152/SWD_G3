using System.Security.Claims;
using EduNexus.Areas.Admin.ViewModels.Profile;
using EduNexus.Constants;
using EduNexus.Extensions;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class ProfileController : Controller
    {
        private readonly IAdminProfileService
            _profileService;

        public ProfileController(
            IAdminProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            long currentUserId =
                User.GetCurrentUserId();

            AdminProfileViewModel? viewModel =
                await _profileService
                    .GetProfileAsync(currentUserId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            long currentUserId =
                User.GetCurrentUserId();

            EditAdminProfileViewModel? viewModel =
                await _profileService
                    .GetEditAsync(currentUserId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            EditAdminProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            long currentUserId =
                User.GetCurrentUserId();

            ServiceResult result =
                await _profileService
                    .UpdateProfileAsync(
                        currentUserId,
                        model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                return View(model);
            }

            /*
             * Refresh ClaimTypes.Name in the authentication
             * cookie so the shared sidebar immediately displays
             * the newly updated full name.
             */
            await RefreshAuthenticationCookieAsync(
                model.FullName.Trim());

            TempData["SuccessMessage"] =
                result.Message;

            return RedirectToAction(
                nameof(Index));
        }

        private async Task
            RefreshAuthenticationCookieAsync(
                string newFullName)
        {
            AuthenticateResult authenticationResult =
                await HttpContext.AuthenticateAsync(
                    CookieAuthenticationDefaults
                        .AuthenticationScheme);

            if (!authenticationResult.Succeeded
                || authenticationResult.Principal == null)
            {
                return;
            }

            List<Claim> claims =
                authenticationResult.Principal
                    .Claims
                    .Where(claim =>
                        claim.Type != ClaimTypes.Name)
                    .ToList();

            claims.Add(
                new Claim(
                    ClaimTypes.Name,
                    newFullName));

            ClaimsIdentity identity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme);

            ClaimsPrincipal principal =
                new ClaimsPrincipal(identity);

            AuthenticationProperties properties =
                authenticationResult.Properties
                ?? new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,
                principal,
                properties);
        }
    }
}