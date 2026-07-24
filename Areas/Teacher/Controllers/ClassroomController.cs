using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Teacher.Controllers
{
    using EduNexus.Constants;

    [Area("Teacher")]
    [Authorize(Roles = RoleNames.Teacher)]
    public class ClassroomController : Controller
    {
        private readonly IClassroomService _service;

        public ClassroomController(IClassroomService service)
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
        public async Task<IActionResult> Create(ClassroomViewModel model)
        {
            long teacherId = 1;

            if (!ModelState.IsValid)
            {
                model.Courses = (await _service.GetCreateModelAsync(teacherId)).Courses;

                return View(model);
            }

            var result = await _service.CreateAsync(model, teacherId);

            if (!result)
            {
                ModelState.AddModelError("", "Cannot create classroom.");

                model.Courses = (await _service.GetCreateModelAsync(teacherId)).Courses;

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            long teacherId = 1;

            var model = await _service.GetByIdAsync(id, teacherId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClassroomViewModel model)
        {
            long teacherId = 1;

            if (!ModelState.IsValid)
            {
                model.Courses = (await _service.GetCreateModelAsync(teacherId)).Courses;

                return View(model);
            }

            var result = await _service.UpdateAsync(model, teacherId);

            if (!result)
            {
                ModelState.AddModelError("", "Cannot update classroom.");

                model.Courses = (await _service.GetCreateModelAsync(teacherId)).Courses;

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(long id)
        {
            long teacherId = 1;

            var model = await _service.GetByIdAsync(id, teacherId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            long teacherId = 1;

            var result = await _service.DeleteAsync(id, teacherId);

            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}