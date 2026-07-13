using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize]
public class QuizController : Controller
{
    private readonly IQuizService _quizService;
    private readonly IQuizAttemptService _quizAttemptService;

    public QuizController(IQuizService quizService, IQuizAttemptService quizAttemptService)
    {
        _quizService = quizService;
        _quizAttemptService = quizAttemptService;
    }

    [Authorize(Roles = RoleNames.Sme)]
    public IActionResult Manage()
    {
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Index()
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var quizzes = await _quizService.GetQuizzesForSmeAsync(smeId);
        return View(quizzes);
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Create()
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _quizService.BuildCreateFormAsync(smeId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Create(QuizFormViewModel model)
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            var form = await _quizService.BuildCreateFormAsync(smeId);
            model.Modules = form.Modules;
            return View(model);
        }

        try
        {
            long quizId = await _quizService.CreateQuizAsync(model, smeId);
            return RedirectToAction(nameof(Questions), new { id = quizId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            var form = await _quizService.BuildCreateFormAsync(smeId);
            model.Modules = form.Modules;

            return View(model);
        }
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Questions(long id, long? bankId)
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _quizService.BuildQuestionSelectionAsync(id, smeId, bankId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Questions(QuizQuestionSelectionViewModel model)
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        try
        {
            await _quizService.UpdateQuizQuestionsAsync(model, smeId);
            TempData["SuccessMessage"] = "Quiz questions updated successfully.";

            return RedirectToAction(nameof(Questions), new
            {
                id = model.QuizId,
                bankId = model.BankId
            });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            var refreshed = await _quizService.BuildQuestionSelectionAsync(
                model.QuizId,
                smeId,
                model.BankId);

            if (refreshed == null)
            {
                return NotFound();
            }

            foreach (var question in refreshed.Questions)
            {
                var posted = model.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId);

                if (posted != null)
                {
                    question.IsSelected = posted.IsSelected;
                    question.Score = posted.Score;
                }
            }

            return View(refreshed);
        }
    }

    [Authorize(Roles = RoleNames.Student)]
    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> NewQuiz()
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _quizAttemptService.GetAvailableQuizzesAsync(studentId);
        return View(model);
    }

    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> History()
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _quizAttemptService.GetHistoryAsync(studentId);
        return View(model);
    }
}