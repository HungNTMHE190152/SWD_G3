using EduNexus.Areas.Admin.ViewModels.UserManagement;
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
    public class UserManagementController : Controller
    {
        private readonly IUserManagementService
            _userManagementService;

        public UserManagementController(
            IUserManagementService userManagementService)
        {
            _userManagementService =
                userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] UserFilterViewModel filter)
        {
            UserManagementIndexViewModel viewModel =
                await _userManagementService
                    .GetUsersAsync(filter);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            UserDetailsViewModel? viewModel =
                await _userManagementService
                    .GetDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateUserViewModel viewModel =
                new CreateUserViewModel
                {
                    IsActive = true,
                    Roles = await _userManagementService
                        .GetRolesAsync()
                };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles =
                    await _userManagementService
                        .GetRolesAsync();

                return View(model);
            }

            ServiceResult<long> result =
                await _userManagementService
                    .CreateUserAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                model.Roles =
                    await _userManagementService
                        .GetRolesAsync();

                return View(model);
            }

            TempData["SuccessMessage"] =
                result.Message;

            return RedirectToAction(
                nameof(Details),
                new { id = result.Data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAccountActive(
            long id,
            bool isActive)
        {
            long currentAdminUserId =
                User.GetCurrentUserId();

            ServiceResult result =
                await _userManagementService
                    .SetAccountActiveAsync(
                        id,
                        isActive,
                        currentAdminUserId);

            SetTempData(result);

            return RedirectToAction(
                nameof(Details),
                new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(
            long id,
            long roleId)
        {
            long currentAdminUserId =
                User.GetCurrentUserId();

            ServiceResult result =
                await _userManagementService
                    .ChangeRoleAsync(
                        id,
                        roleId,
                        currentAdminUserId);

            SetTempData(result);

            return RedirectToAction(
                nameof(Details),
                new { id });
        }

        private void SetTempData(ServiceResult result)
        {
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] =
                    result.Message;
            }
            else
            {
                TempData["ErrorMessage"] =
                    result.Message;
            }
        }
    }
}