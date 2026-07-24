using EduNexus.Constants;
using EduNexus.Services.Assignment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = RoleNames.Teacher)]
    public class AssignmentSubmissionController : Controller
    {
        private readonly IAssignmentSubmissionService _service;

        public AssignmentSubmissionController(
            IAssignmentSubmissionService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(long classroomAssignmentId)
        {
            long teacherId = 1;

            var model = await _service.GetByAssignmentAsync(
                classroomAssignmentId,
                teacherId);

            ViewBag.ClassroomAssignmentId = classroomAssignmentId;

            return View(model);
        }
    }
}