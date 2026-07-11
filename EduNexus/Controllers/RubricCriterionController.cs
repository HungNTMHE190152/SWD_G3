
using Microsoft.AspNetCore.Mvc;
using EduNexus.Services;
using EduNexus.ViewModels.RubricCriterion;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Rubric;

namespace EduNexus.Controllers
{
    public class RubricCriterionController : Controller
    {
        private readonly IRubricCriterionService _criterionService;

        public RubricCriterionController(IRubricCriterionService criterionService)
        {
            _criterionService = criterionService;
        }

        // GET: RubricCriterion/Index?rubricId=5
        public async Task<IActionResult> Index(long rubricId)
        {
            var model = await _criterionService.GetCriteriaAsync(rubricId);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: Thêm criterion mới (AJAX hoặc form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCriterionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _criterionService.GetCriteriaAsync(model.RubricId);
                return View("Index", vm);
            }

            try
            {
                await _criterionService.AddCriterionAsync(model);
                return RedirectToAction(nameof(Index), new { rubricId = model.RubricId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var vm = await _criterionService.GetCriteriaAsync(model.RubricId);
                return View("Index", vm);
            }
        }

        // GET: Edit
        public async Task<IActionResult> Edit(long id)
        {
            var model = await _criterionService.GetForEditAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCriterionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _criterionService.UpdateCriterionAsync(model);
                return RedirectToAction(nameof(Index), new { rubricId = model.RubricId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // POST: Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(long criterionId, long rubricId)
        {
            await _criterionService.RemoveCriterionAsync(criterionId);
            return RedirectToAction(nameof(Index), new { rubricId });
        }

        // POST: Save all changes (inline editing)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveChanges(RubricManagementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                await _criterionService.UpdateCriteriaAsync(model.CurrentCriteria);
                return RedirectToAction(nameof(Index), new { rubricId = model.RubricId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", model);
            }
        }
    }
}