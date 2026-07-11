using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Student;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduNexus.Controllers
{
    public class StudentAssignmentController : Controller
    {
        private readonly IStudentAssignmentService _studentAssignmentService;
        private readonly IAIGradingService _aiGradingService;
        private object newSubmission;

        public StudentAssignmentController(IStudentAssignmentService studentAssignmentService,IAIGradingService aIGradingService)
        {
            _studentAssignmentService = studentAssignmentService;
            _aiGradingService = aIGradingService;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy StudentId từ User hiện tại (bạn có thể điều chỉnh theo cách lấy UserId của project)
            long studentId = GetCurrentStudentId(); // TODO: Implement theo Auth của bạn

            var assignments = await _studentAssignmentService.GetAssignmentsAsync(studentId);

            return View(assignments);
        }

        public async Task<IActionResult> Details(long assignmentId)
        {
            long studentId = GetCurrentStudentId();

            var model = await _studentAssignmentService.GetAssignmentDetailAsync(assignmentId, studentId);

            if (model == null)
                return NotFound("Bạn không có quyền xem Assignment này hoặc Assignment không tồn tại.");

            Console.WriteLine($"assignmentId = {assignmentId}");
            Console.WriteLine($"studentId = {studentId}");

            return View(model);
        }

        public async Task<IActionResult> Start(long assignmentId)
        {
            long studentId = GetCurrentStudentId();
            var model = await _studentAssignmentService.StartAssignmentAsync(assignmentId, studentId);

            if (model == null)
                return NotFound("Không thể bắt đầu Assignment này.");

            return View("Start", model); // Views/StudentAssignment/Start.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(StudentAssignmentWorkspaceViewModel model)
        {
            long studentId = GetCurrentStudentId();

            if (model == null || model.SubmissionId == 0)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                // Trả lại dữ liệu người dùng vừa nhập
                return View("Start", model);
            }

            await _studentAssignmentService.SaveDraftAsync(model, studentId);

            TempData["Success"] = "Đã lưu nháp thành công!";
            return RedirectToAction(nameof(Start), new { assignmentId = model.AssignmentId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAssignment(long assignmentId)
        {
            long studentId = GetCurrentStudentId();

            bool success = await _studentAssignmentService.SubmitAssignmentAsync(assignmentId, studentId);

            if (success)
            {
                // === GỌI AI CHẤM BÀI NGAY SAU KHI NỘP ===
                try
                {
                    // Lấy submission vừa nộp
                    var latestSubmission = await _studentAssignmentService.GetLatestSubmissionAsync(assignmentId, studentId);

                    if (latestSubmission != null)
                    {
                        await _aiGradingService.GradeSubmissionAsync(latestSubmission.SubmissionId);
                        TempData["Success"] = "Nộp bài thành công! AI đang chấm bài...";
                    }
                    else
                    {
                        TempData["Success"] = "Nộp bài thành công!";
                    }
                }
                catch (Exception ex)
                {
                    // Vẫn coi là nộp thành công dù AI lỗi
                    Console.WriteLine($"AI Grading Error: {ex.Message}");
                    TempData["Success"] = "Nộp bài thành công! (AI chấm sẽ thực hiện sau)";
                }
            }
            else
            {
                TempData["Error"] = "Không thể nộp bài. Vui lòng kiểm tra thời hạn hoặc trạng thái bài làm.";
            }

            return RedirectToAction(nameof(Details), new { assignmentId });
        }
        public async Task<IActionResult> MySubmissions()
        {
            long studentId = GetCurrentStudentId();
            var model = await _studentAssignmentService.GetSubmissionHistoryAsync(studentId);
            return View(model);
        }

        public async Task<IActionResult> SubmissionDetail(long submissionId)
        {
            long studentId = GetCurrentStudentId();
            var model = await _studentAssignmentService.GetSubmissionDetailAsync(submissionId, studentId);

            if (model == null)
                return NotFound("Bạn không có quyền xem Submission này.");

            return View(model);
        }

        // ==================== AI RESULT ====================
        
        // ==================== AI SUBMISSION RESULT ====================
        

        private long GetCurrentStudentId()
        {
            return long.TryParse(
        User.FindFirstValue(ClaimTypes.NameIdentifier),
        out long id)
        ? id
        : 0;
        }
    }
}