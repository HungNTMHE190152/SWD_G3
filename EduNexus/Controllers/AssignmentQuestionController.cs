using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.AssignmentQuestion;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers
{
    public class AssignmentQuestionController : Controller
    {
        private readonly IAssignmentQuestionService _service;

        public AssignmentQuestionController(IAssignmentQuestionService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(long assignmentId)
        {
            var model = await _service.GetAssignmentQuestionsAsync(assignmentId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion(long assignmentId, long questionId)
        {
            await _service.AddQuestionAsync(assignmentId, questionId);

            return RedirectToAction(nameof(Index),
                new { assignmentId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveQuestion(long assignmentQuestionId,
            long assignmentId)
        {
            await _service.RemoveQuestionAsync(assignmentQuestionId);

            return RedirectToAction(nameof(Index),
                new { assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuestion(AssignmentQuestionItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index),
                    new { assignmentId = model.AssignmentId });
            }

            await _service.UpdateQuestionAsync(model);

            return RedirectToAction(nameof(Index),
                new { assignmentId = model.AssignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveChanges(AssignmentQuestionManagementViewModel model)
        {
            await _service.UpdateQuestionsAsync(model.CurrentQuestions);

            return RedirectToAction(nameof(Index),
                new { assignmentId = model.AssignmentId });
        }
    }
}