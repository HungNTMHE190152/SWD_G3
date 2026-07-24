using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.SME.Controllers
{
    [Area("SME")]
    [Authorize(Roles = "SME")]
    public class AssignmentTemplateController : Controller
    {
        private readonly IAssignmentTemplateService _service;

        public AssignmentTemplateController(
            IAssignmentTemplateService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            long smeId = 1;

            var model = await _service.GetAllAsync(smeId);

            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            long smeId = 1;

            var model = new AssignmentTemplateFormViewModel
            {
                Courses = await _service.GetCoursesAsync(smeId)
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
        AssignmentTemplateFormViewModel model)
        {
            long smeId = 1;

            if (!ModelState.IsValid)
            {
                model.Courses = await _service.GetCoursesAsync(smeId);

                if (model.CourseId > 0)
                {
                    model.Modules = await _service.GetModulesAsync(model.CourseId);
                }

                return View(model);
            }

            var result = await _service.CreateAsync(model, smeId);

            if (!result)
            {
                ModelState.AddModelError("", "Cannot create Assignment Template.");

                model.Courses = await _service.GetCoursesAsync(smeId);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(long id)
        {
            long smeId = 1;

            var model = await _service.GetByIdAsync(id, smeId);

            if (model == null)
                return NotFound();

            model.Courses = await _service.GetCoursesAsync(smeId);

            model.Modules = await _service.GetModulesAsync(model.CourseId);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
        AssignmentTemplateFormViewModel model)
        {
            long smeId = 1;

            if (!ModelState.IsValid)
            {
                model.Courses = await _service.GetCoursesAsync(smeId);
                model.Modules = await _service.GetModulesAsync(model.CourseId);

                return View(model);
            }

            var result = await _service.UpdateAsync(model, smeId);

            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            long smeId = 1;

            var result = await _service.DeleteAsync(id, smeId);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetModules(long courseId)
        {
            var modules = await _service.GetModulesAsync(courseId);

            return Json(modules);
        }
    }
}