using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize(Roles = RoleNames.Sme)]
[AllowAnonymous]
public class QuestionController : Controller
{
    private readonly IQuestionService _questionService;
    private readonly long demoSmeId = 2;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    public async Task<IActionResult> Index(long bankId)
    {
        var questions = await _questionService.GetQuestionsByBankAsync(bankId);
        ViewBag.BankId = bankId;
        return View(questions);
    }

    [HttpGet]
    public IActionResult Create(long bankId)
    {
        var model = new QuestionFormViewModel { BankId = bankId };
        // Pre-fill a choice for convenience
        model.Choices.Add(new ChoiceFormViewModel { ChoiceContent = "Option 1", IsCorrect = true, DisplayOrder = 1 });
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestionFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.QuestionContent))
            ModelState.AddModelError("QuestionContent", "Question content cannot be empty.");
        
        if (model.Choices == null || model.Choices.Count == 0)
            ModelState.AddModelError("", "At least one choice is required.");
        else if (model.QuestionType == "Single Choice" && model.Choices.Count(c => c.IsCorrect) != 1)
            ModelState.AddModelError("", "Single Choice questions must have exactly one correct answer.");

        if (ModelState.IsValid)
        {
            await _questionService.CreateQuestionAsync(model, demoSmeId);
            return RedirectToAction(nameof(Index), new { bankId = model.BankId });
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var model = await _questionService.GetQuestionForEditAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(QuestionFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.QuestionContent))
            ModelState.AddModelError("QuestionContent", "Question content cannot be empty.");
        
        if (model.Choices == null || model.Choices.Count == 0)
            ModelState.AddModelError("", "At least one choice is required.");
        else if (model.QuestionType == "Single Choice" && model.Choices.Count(c => c.IsCorrect) != 1)
            ModelState.AddModelError("", "Single Choice questions must have exactly one correct answer.");

        if (ModelState.IsValid)
        {
            await _questionService.UpdateQuestionAsync(model);
            return RedirectToAction(nameof(Index), new { bankId = model.BankId });
        }
        return View(model);
    }
}
