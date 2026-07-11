using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize(Roles = RoleNames.Student)]
[Authorize(Roles = RoleNames.Student)]
public class QuizAttemptController : Controller
{
    private readonly IQuizAttemptService _attemptService;
    private readonly long demoStudentId = 1;

    public QuizAttemptController(IQuizAttemptService attemptService)
    {
        _attemptService = attemptService;
    }

    public async Task<IActionResult> Index()
    {
        var quizzes = await _attemptService.GetAvailableQuizzesForStudentAsync(demoStudentId);
        return View(quizzes);
    }

    [HttpGet]
    public async Task<IActionResult> NewQuiz(long id)
    {
        var details = await _attemptService.GetQuizDetailsForStudentAsync(id, demoStudentId);
        if (details == null) return NotFound();
        return View(details);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(long quizId)
    {
        var attemptViewModel = await _attemptService.StartQuizAsync(quizId, demoStudentId);
        return RedirectToAction(nameof(Take), new { attemptId = attemptViewModel.AttemptId });
    }

    [HttpGet]
    public async Task<IActionResult> Take(long attemptId)
    {
        var model = await _attemptService.GetQuizTakingModelAsync(attemptId, demoStudentId);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(QuizSubmitViewModel model)
    {
        var success = await _attemptService.SubmitQuizAsync(model, demoStudentId);
        if (success)
        {
            return RedirectToAction("Result", "QuizResult", new { attemptId = model.AttemptId });
        }
        return BadRequest();
    }
}
