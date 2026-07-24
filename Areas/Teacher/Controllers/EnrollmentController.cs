using EduNexus.Constants;
using EduNexus.Services.Assignment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = RoleNames.Teacher)]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _service;

        public EnrollmentController(IEnrollmentService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(long classroomId)
        {
            long teacherId = 1;

            var model = await _service.GetByClassroomAsync(classroomId, teacherId);

            ViewBag.ClassroomId = classroomId;

            return View(model);
        }
    }
}