using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.Choice;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class ChoiceController : Controller
{
    private readonly IChoiceService _choiceService;
    private readonly IQuestionService _questionService;

    public ChoiceController(
        IChoiceService choiceService,
        IQuestionService questionService)
    {
        _choiceService = choiceService;
        _questionService = questionService;
    }

    //=========================================
    // LIST
    //=========================================

    public async Task<IActionResult> Index(long questionId)
    {
        ViewBag.QuestionId = questionId;

        var choices =
            await _choiceService.GetByQuestionAsync(questionId);

        return View(choices);
    }

    //=========================================
    // CREATE
    //=========================================

    [HttpGet]
    public IActionResult Create(long questionId)
    {
        return View(new ChoiceViewModel
        {
            QuestionId = questionId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ChoiceViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        // Chỉ cho phép 1 đáp án đúng
        if (vm.IsCorrect)
        {
            var oldChoices =
                await _choiceService.GetByQuestionAsync(vm.QuestionId);

            foreach (var c in oldChoices)
            {
                if (c.IsCorrect)
                {
                    c.IsCorrect = false;
                    await _choiceService.UpdateAsync(c);
                }
            }
        }

        Choice choice = new()
        {
            QuestionId = vm.QuestionId,
            ChoiceContent = vm.ChoiceContent,
            DisplayOrder = vm.DisplayOrder,
            IsCorrect = vm.IsCorrect
        };

        await _choiceService.CreateAsync(choice);

        return RedirectToAction(nameof(Index),
            new
            {
                questionId = vm.QuestionId
            });
    }

    //=========================================
    // EDIT
    //=========================================

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var choice =
            await _choiceService.GetByIdAsync(id);

        if (choice == null)
            return NotFound();

        ChoiceViewModel vm = new()
        {
            ChoiceId = choice.ChoiceId,
            QuestionId = choice.QuestionId,
            ChoiceContent = choice.ChoiceContent,
            DisplayOrder = choice.DisplayOrder,
            IsCorrect = choice.IsCorrect
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ChoiceViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var choice =
            await _choiceService.GetByIdAsync(vm.ChoiceId);

        if (choice == null)
            return NotFound();

        if (vm.IsCorrect)
        {
            var oldChoices =
                await _choiceService.GetByQuestionAsync(vm.QuestionId);

            foreach (var c in oldChoices)
            {
                c.IsCorrect = false;
                await _choiceService.UpdateAsync(c);
            }
        }

        choice.ChoiceContent = vm.ChoiceContent;
        choice.DisplayOrder = vm.DisplayOrder;
        choice.IsCorrect = vm.IsCorrect;

        await _choiceService.UpdateAsync(choice);

        return RedirectToAction(nameof(Index),
            new
            {
                questionId = vm.QuestionId
            });
    }

    //=========================================
    // DELETE
    //=========================================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        var choice =
            await _choiceService.GetByIdAsync(id);

        if (choice == null)
            return NotFound();

        long questionId = choice.QuestionId;

        await _choiceService.DeleteAsync(id);

        return RedirectToAction(nameof(Index),
            new
            {
                questionId
            });
    }
}