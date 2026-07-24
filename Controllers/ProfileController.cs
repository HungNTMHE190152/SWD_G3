using System.Security.Claims;
using EduNexus.Extensions;
using EduNexus.Services.Common;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService
            _profileService;

        public ProfileController(
            IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            long currentUserId =
                User.GetCurrentUserId();

            UserProfileViewModel? viewModel =
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

            EditProfileViewModel? viewModel =
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
            EditProfileViewModel model)
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