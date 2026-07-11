// 1. RubricController.cs
// Đường dẫn: Controllers/RubricController.cs

using Microsoft.AspNetCore.Mvc;
using EduNexus.Services;
using EduNexus.ViewModels.Rubric;

namespace EduNexus.Controllers
{
    public class RubricController : Controller
    {
        private readonly IRubricService _rubricService;

        public RubricController(IRubricService rubricService)
        {
            _rubricService = rubricService;
        }

        // GET: Rubric/Index?assignmentId=5
        public async Task<IActionResult> Index(long assignmentId)
        {
            var rubrics = await _rubricService.GetRubricsAsync(assignmentId);
            ViewData["AssignmentId"] = assignmentId;
            return View(rubrics);
        }

        // GET: Rubric/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var rubric = await _rubricService.GetByIdAsync(id);
            if (rubric == null)
            {
                return NotFound();
            }
            return View(rubric);
        }

        // GET: Rubric/Create?assignmentId=5
        public async Task<IActionResult> Create(long assignmentId)
        {
            var model = await _rubricService.GetCreateModelAsync(assignmentId);
            return View(model);
        }

        // POST: Rubric/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRubricViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _rubricService.CreateAsync(model);
                return RedirectToAction(nameof(Index), new { assignmentId = model.AssignmentId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: Rubric/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var model = await _rubricService.GetEditModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Rubric/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRubricViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _rubricService.UpdateAsync(model);
                return RedirectToAction(nameof(Index), new { assignmentId = model.AssignmentId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: Rubric/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var model = await _rubricService.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Rubric/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var rubric = await _rubricService.GetByIdAsync(id);
            if (rubric == null)
            {
                return NotFound();
            }

            await _rubricService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { assignmentId = rubric.AssignmentId });
        }
    }
}