using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.SME.Controllers
{
    [Area("SME")]
    [Authorize(Roles = "SME")]
    public class RubricController : Controller
    {
        private readonly IRubricService _service;

        public RubricController(IRubricService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            long smeId = 1; // TODO: Lấy từ Claims khi tích hợp Authentication

            var data = await _service.GetAllAsync(smeId);

            return View(data);
        }

        public async Task<IActionResult> Create(long assignmentId)
        {
            var model = await _service.GetCreateModelAsync(assignmentId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RubricViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _service.CreateAsync(model);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Total score of all criteria must equal Assignment Total Score."
                );

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var rubric = await _service.GetByIdAsync(id);

            if (rubric == null)
                return NotFound();

            return View(rubric);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RubricViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _service.UpdateAsync(model);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Total score of all criteria must equal Assignment Total Score."
                );

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}