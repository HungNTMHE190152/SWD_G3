using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.Question;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class QuestionController : Controller
{
    private readonly IQuestionService _questionService;
    private readonly IQuestionBankService _questionBankService;

    public QuestionController(
        IQuestionService questionService,
        IQuestionBankService questionBankService)
    {
        _questionService = questionService;
        _questionBankService = questionBankService;
    }

    //=====================================
    // LIST
    //=====================================

    public async Task<IActionResult> Index(long questionBankId)
    {
        ViewBag.QuestionBankId = questionBankId;

        var questions =
            await _questionService.GetByBankAsync(questionBankId);

        return View(questions);
    }

    //=====================================
    // DETAILS
    //=====================================

    public async Task<IActionResult> Details(long id)
    {
        var question =
            await _questionService.GetByIdAsync(id);

        if (question == null)
            return NotFound();

        return View(question);
    }

    //=====================================
    // CREATE
    //=====================================

    [HttpGet]
    public IActionResult Create(long questionBankId)
    {
        QuestionViewModel vm = new()
        {
            QuestionBankId = questionBankId
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        Question question = new()
        {
            QuestionBankId = vm.QuestionBankId,
            QuestionContent = vm.QuestionContent,
            QuestionType = vm.QuestionType,
            Difficulty = vm.Difficulty,
            Explanation = vm.Explanation,
            CreatedBy = 1
        };

        await _questionService.CreateAsync(question);

        return RedirectToAction(
            nameof(Index),
            new
            {
                questionBankId = vm.QuestionBankId
            });
    }

    //=====================================
    // EDIT
    //=====================================

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var question =
            await _questionService.GetByIdAsync(id);

        if (question == null)
            return NotFound();

        QuestionViewModel vm = new()
        {
            QuestionId = question.QuestionId,
            QuestionBankId = question.QuestionBankId,
            QuestionContent = question.QuestionContent,
            QuestionType = question.QuestionType,
            Difficulty = question.Difficulty,
            Explanation = question.Explanation
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(QuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var question =
            await _questionService.GetByIdAsync(vm.QuestionId);

        if (question == null)
            return NotFound();

        question.QuestionContent = vm.QuestionContent;
        question.QuestionType = vm.QuestionType;
        question.Difficulty = vm.Difficulty;
        question.Explanation = vm.Explanation;

        await _questionService.UpdateAsync(question);

        return RedirectToAction(
            nameof(Index),
            new
            {
                questionBankId = question.QuestionBankId
            });
    }

    //=====================================
    // DELETE
    //=====================================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        var question =
            await _questionService.GetByIdAsync(id);

        if (question == null)
            return NotFound();

        long bankId = question.QuestionBankId;

        await _questionService.DeleteAsync(id);

        return RedirectToAction(
            nameof(Index),
            new
            {
                questionBankId = bankId
            });
    }
}