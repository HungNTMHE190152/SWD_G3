using Microsoft.AspNetCore.Mvc;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Assignment;

namespace EduNexus.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        public async Task<IActionResult> Index()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync();

            return View(assignments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _assignmentService.CreateAssignmentAsync(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(long id)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }
        public async Task<IActionResult> Edit(long id)
        {
            var model = await _assignmentService.GetAssignmentForEditAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _assignmentService.UpdateAssignmentAsync(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id)
        {
            var model = await _assignmentService.GetAssignmentByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _assignmentService.DeleteAssignmentAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}