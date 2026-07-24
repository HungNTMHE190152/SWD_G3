using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.QuizTemplate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class QuizTemplateController : Controller
{
    private readonly IQuizTemplateService _quizTemplateService;

    public QuizTemplateController(IQuizTemplateService quizTemplateService)
    {
        _quizTemplateService = quizTemplateService;
    }

    //=========================================
    // LIST
    //=========================================

    public async Task<IActionResult> Index()
    {
        var quizzes = await _quizTemplateService.GetAllAsync();

        return View(quizzes);
    }

    //=========================================
    // DETAILS
    //=========================================

    public async Task<IActionResult> Details(long id)
    {
        var quiz = await _quizTemplateService.GetByIdAsync(id);

        if (quiz == null)
            return NotFound();

        return View(quiz);
    }

    //=========================================
    // CREATE
    //=========================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Courses = new SelectList(
            await _quizTemplateService.GetCoursesAsync(),
            "CourseId",
            "Title");

        ViewBag.Modules = new SelectList(
            new List<Module>(),
            "ModuleId",
            "Title");

        return View(new QuizTemplateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuizTemplateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Courses = new SelectList(
                await _quizTemplateService.GetCoursesAsync(),
                "CourseId",
                "Title",
                vm.CourseId);

            ViewBag.Modules = new SelectList(
                await _quizTemplateService.GetModulesAsync(vm.CourseId),
                "ModuleId",
                "Title",
                vm.ModuleId);

            return View(vm);
        }

        QuizTemplate quiz = new()
        {
            CourseId = vm.CourseId,
            ModuleId = vm.ModuleId,
            CreatedBy = 1,
            Title = vm.Title,
            Description = vm.Description,
            DefaultDurationMinutes = vm.DefaultDurationMinutes,
            DefaultPassingScore = vm.DefaultPassingScore,
            DefaultMaxAttempts = vm.DefaultMaxAttempts,
            ShuffleQuestions = vm.ShuffleQuestions,
            ShuffleAnswers = vm.ShuffleAnswers,
            IsActive = vm.IsActive
        };

        await _quizTemplateService.CreateAsync(quiz);

        return RedirectToAction(nameof(Index));
    }

    //=========================================
    // EDIT
    //=========================================

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var quiz = await _quizTemplateService.GetByIdAsync(id);

        if (quiz == null)
            return NotFound();

        ViewBag.Courses = new SelectList(
            await _quizTemplateService.GetCoursesAsync(),
            "CourseId",
            "Title",
            quiz.CourseId);

        ViewBag.Modules = new SelectList(
            await _quizTemplateService.GetModulesAsync(quiz.CourseId),
            "ModuleId",
            "Title",
            quiz.ModuleId);

        QuizTemplateViewModel vm = new()
        {
            QuizTemplateId = quiz.QuizTemplateId,
            CourseId = quiz.CourseId,
            ModuleId = quiz.ModuleId,
            Title = quiz.Title,
            Description = quiz.Description,
            DefaultDurationMinutes = quiz.DefaultDurationMinutes,
            DefaultPassingScore = quiz.DefaultPassingScore,
            DefaultMaxAttempts = quiz.DefaultMaxAttempts,
            ShuffleQuestions = quiz.ShuffleQuestions,
            ShuffleAnswers = quiz.ShuffleAnswers,
            IsActive = quiz.IsActive
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(QuizTemplateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Courses = new SelectList(
                await _quizTemplateService.GetCoursesAsync(),
                "CourseId",
                "Title",
                vm.CourseId);

            ViewBag.Modules = new SelectList(
                await _quizTemplateService.GetModulesAsync(vm.CourseId),
                "ModuleId",
                "Title",
                vm.ModuleId);

            return View(vm);
        }

        var quiz = await _quizTemplateService.GetByIdAsync(vm.QuizTemplateId);

        if (quiz == null)
            return NotFound();

        quiz.CourseId = vm.CourseId;
        quiz.ModuleId = vm.ModuleId;
        quiz.Title = vm.Title;
        quiz.Description = vm.Description;
        quiz.DefaultDurationMinutes = vm.DefaultDurationMinutes;
        quiz.DefaultPassingScore = vm.DefaultPassingScore;
        quiz.DefaultMaxAttempts = vm.DefaultMaxAttempts;
        quiz.ShuffleQuestions = vm.ShuffleQuestions;
        quiz.ShuffleAnswers = vm.ShuffleAnswers;
        quiz.IsActive = vm.IsActive;

        await _quizTemplateService.UpdateAsync(quiz);

        return RedirectToAction(nameof(Index));
    }

    //=========================================
    // DELETE
    //=========================================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        await _quizTemplateService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}