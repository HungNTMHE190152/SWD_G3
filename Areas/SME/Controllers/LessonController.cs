using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.Lesson;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class LessonController : Controller
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    //==========================
    // LIST
    //==========================

    public async Task<IActionResult> Index(long moduleId)
    {
        ViewBag.ModuleId = moduleId;

        var lessons = await _lessonService.GetByModuleAsync(moduleId);

        return View(lessons);
    }

    //==========================
    // CREATE
    //==========================

    [HttpGet]
    public IActionResult Create(long moduleId)
    {
        LessonViewModel vm = new()
        {
            ModuleId = moduleId,
            DisplayOrder = 1
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LessonViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Lesson lesson = new()
        {
            ModuleId = vm.ModuleId,
            Title = vm.Title,
            Content = vm.Content,
            LessonType = vm.LessonType,
            EstimatedMinutes = vm.EstimatedMinutes,
            DisplayOrder = vm.DisplayOrder,
            Status = "DRAFT",
            CreatedAt = DateTime.Now
        };

        bool result = await _lessonService.CreateAsync(lesson);

        if (!result)
        {
            TempData["Error"] = "Create lesson failed.";
            return View(vm);
        }

        TempData["Success"] = "Lesson created successfully.";

        return RedirectToAction(
            "Details",
            "Module",
            new
            {
                area = "SME",
                id = vm.ModuleId
            });
    }

    //==========================
    // DETAILS
    //==========================

    public async Task<IActionResult> Details(long id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);

        if (lesson == null)
            return NotFound();

        return View(lesson);
    }

    //==========================
    // EDIT
    //==========================

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);

        if (lesson == null)
            return NotFound();

        LessonViewModel vm = new()
        {
            LessonId = lesson.LessonId,
            ModuleId = lesson.ModuleId,
            Title = lesson.Title,
            Content = lesson.Content,
            LessonType = lesson.LessonType,
            DisplayOrder = lesson.DisplayOrder,
            EstimatedMinutes = lesson.EstimatedMinutes
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LessonViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var lesson = await _lessonService.GetByIdAsync(vm.LessonId);

        if (lesson == null)
            return NotFound();

        lesson.Title = vm.Title;
        lesson.Content = vm.Content;
        lesson.LessonType = vm.LessonType;
        lesson.DisplayOrder = vm.DisplayOrder;
        lesson.EstimatedMinutes = vm.EstimatedMinutes;

        await _lessonService.UpdateAsync(lesson);

        return RedirectToAction(
            "Details",
            "Module",
            new
            {
                area = "SME",
                id = lesson.ModuleId
            });
    }

    //==========================
    // DELETE
    //==========================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);

        if (lesson == null)
            return NotFound();

        long moduleId = lesson.ModuleId;

        await _lessonService.DeleteAsync(id);

        return RedirectToAction(
            "Details",
            "Module",
            new
            {
                area = "SME",
                id = moduleId
            });
    }
    public async Task<IActionResult> Publish(long id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);

        if (lesson == null)
            return NotFound();

        lesson.Status = "PUBLISHED";

        await _lessonService.UpdateAsync(lesson);

        return RedirectToAction(nameof(Details),
            new { id = lesson.LessonId });
    }
}