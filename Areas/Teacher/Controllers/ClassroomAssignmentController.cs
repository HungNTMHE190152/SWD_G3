using EduNexus.Constants;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = RoleNames.Teacher)]
    public class ClassroomAssignmentController : Controller
    {
        private readonly IClassroomAssignmentService _service;

        public ClassroomAssignmentController(IClassroomAssignmentService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            long teacherId = 1;

            var model = await _service.GetAllAsync(teacherId);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            long teacherId = 1;

            var model = await _service.GetCreateModelAsync(teacherId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassroomAssignmentViewModel model)
        {
            long teacherId = 1;

            if (!ModelState.IsValid)
            {
                await _service.PopulateDropdownsAsync(model);
                return View(model);
            }

            var result = await _service.CreateAsync(model, teacherId);

            if (!result)
            {
                ModelState.AddModelError("", "Cannot publish assignment.");

                await _service.PopulateDropdownsAsync(model);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var model = await _service.GetByIdAsync(id);

            if (model == null)
                return NotFound();

            await _service.PopulateDropdownsAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClassroomAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _service.PopulateDropdownsAsync(model);

                return View(model);
            }

            var result = await _service.UpdateAsync(model);

            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(long id)
        {
            var model = await _service.GetByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}