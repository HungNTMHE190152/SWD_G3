using EduNexus.Constants;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = RoleNames.Teacher)]
    public class SubmissionGradeController : Controller
    {
        private readonly ISubmissionGradeService _service;

        public SubmissionGradeController(
            ISubmissionGradeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Edit(long submissionId)
        {
            var model = await _service.GetSubmissionAsync(submissionId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubmissionGradeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            long teacherId = 1;

            await _service.SaveGradeAsync(model, teacherId);

            return RedirectToAction(
                "Index",
                "AssignmentSubmission",
                new
                {
                    classroomAssignmentId = model.ClassroomAssignmentId
                });
        }
    }
}