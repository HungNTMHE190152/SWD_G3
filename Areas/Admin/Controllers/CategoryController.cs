using EduNexus.Areas.Admin.ViewModels.Category;
using EduNexus.Constants;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] CategoryFilterViewModel filter)
        {
            CategoryIndexViewModel viewModel =
                await _categoryService
                    .GetCategoriesAsync(filter);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(
                new CategoryFormViewModel
                {
                    IsActive = true
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ServiceResult<long> result =
                await _categoryService
                    .CreateAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                return View(model);
            }

            TempData["SuccessMessage"] =
                result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            CategoryFormViewModel? viewModel =
                await _categoryService
                    .GetForEditAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ServiceResult result =
                await _categoryService
                    .UpdateAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                return View(model);
            }

            TempData["SuccessMessage"] =
                result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(
    long id,
    bool isActive)
        {
            ServiceResult result =
                await _categoryService.SetActiveAsync(
                    id,
                    isActive);

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

            return RedirectToAction(nameof(Index));
        }
    }
}