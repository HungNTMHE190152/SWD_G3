using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize]
[AllowAnonymous]
public class QuizController : Controller
{
    private readonly IQuizService _quizService;
    private readonly long demoSmeId = 2;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    public async Task<IActionResult> Manage(long assignmentId = 1) // Default to assignment 1 for demo
    {
        var quizzes = await _quizService.GetQuizzesByAssignmentAsync(assignmentId);
        ViewBag.AssignmentId = assignmentId;
        return View(quizzes);
    }

    [HttpGet]
    public IActionResult Create(long assignmentId)
    {
        var model = new QuizFormViewModel { AssignmentId = assignmentId };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuizFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _quizService.CreateQuizAsync(model, demoSmeId);
            return RedirectToAction(nameof(Manage), new { assignmentId = model.AssignmentId });
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var model = await _quizService.GetQuizByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(QuizFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _quizService.UpdateQuizAsync(model);
            return RedirectToAction(nameof(Manage), new { assignmentId = model.AssignmentId });
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> SelectQuestions(long quizId, long bankId = 1)
    {
        var model = await _quizService.GetQuizQuestionSelectionAsync(quizId, bankId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddQuestions(QuizQuestionSelectionViewModel model)
    {
        if (model.SelectedQuestionIds != null && model.SelectedQuestionIds.Count > 0)
        {
            await _quizService.AddQuestionsToQuizAsync(model.QuizId, model.SelectedQuestionIds);
        }
        return RedirectToAction(nameof(SelectQuestions), new { quizId = model.QuizId, bankId = model.BankId });
    }
}