using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Student)]
public class QuizAttemptController : Controller
{
    private readonly IQuizAttemptService _quizAttemptService;

    public QuizAttemptController(IQuizAttemptService quizAttemptService)
    {
        _quizAttemptService = quizAttemptService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(long quizId)
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        try
        {
            long attemptId = await _quizAttemptService.StartAttemptAsync(quizId, studentId);
            return RedirectToAction(nameof(Taking), new { id = attemptId });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("NewQuiz", "Quiz");
        }
    }

    public async Task<IActionResult> Taking(long id)
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _quizAttemptService.GetTakingAsync(id, studentId);

        if (model == null)
        {
            return RedirectToAction("Result", "QuizResult", new { id });
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(QuizSubmitViewModel model)
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        try
        {
            long attemptId = await _quizAttemptService.SubmitAsync(model, studentId);
            return RedirectToAction("Result", "QuizResult", new { id = attemptId });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Taking), new { id = model.AttemptId });
        }
    }
}