using EduNexus.Constants;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduNexus.Helpers;
namespace EduNexus.Controllers;

[Authorize]
public class LessonController : Controller
{
    private readonly ILessonService _lessonService;
    private readonly IModuleService _moduleService;
    private readonly IProgressService _progressService;

    public LessonController(
        ILessonService lessonService,
        IModuleService moduleService,
        IProgressService progressService)
    {
        _lessonService = lessonService;
        _moduleService = moduleService;
        _progressService = progressService;
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Index(long moduleId)
    {
        var lessons = await _lessonService.GetLessonsByModuleAsync(moduleId);
        ViewBag.ModuleId = moduleId;

        return View(lessons);
    }

    [Authorize(Roles = RoleNames.Sme)]
    public IActionResult Create(long moduleId)
    {
        var lesson = new Lesson
        {
            ModuleId = moduleId,
            LessonType = "MARKDOWN",
            DisplayOrder = 1,
            Duration = 0,
            IsPublished = false
        };

        return View(lesson);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Create(Lesson lesson, bool isPublish)
    {
        ModelState.Remove("Module");
        ModelState.Remove("Resources");
        ModelState.Remove("Progresses");
        ModelState.Remove("LessonTranscripts");

        if (!ModelState.IsValid)
        {
            return View(lesson);
        }

        lesson.IsPublished = isPublish;
        lesson.CreatedAt = DateTime.Now;

        await _lessonService.CreateLessonAsync(lesson);

        return RedirectToAction("Index", new { moduleId = lesson.ModuleId });
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Publish(long lessonId, long moduleId)
    {
        await _lessonService.PublishLessonAsync(lessonId);
        return RedirectToAction("Index", new { moduleId });
    }

    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> ViewLesson(long id)
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var canView = await _progressService.CanStudentViewLessonAsync(studentId, id);

        if (!canView)
        {
            return Forbid();
        }

        var lesson = await _lessonService.GetLessonDetailAsync(id);

        if (lesson == null)
        {
            return NotFound();
        }

        return View("MyLessons", lesson);
    }
    [HttpPost]
    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> Complete(long lessonId)
    {
        long demoStudentId = 1;

        await _progressService.MarkLessonCompletedAsync(demoStudentId, lessonId);
        return RedirectToAction("ViewLesson", new { id = lessonId });
    }
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Edit(long id)
    {
        var lesson = await _lessonService.GetLessonDetailAsync(id);

        if (lesson == null)
        {
            return NotFound();
        }

        return View(lesson);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Edit(Lesson lesson, bool isPublish)
    {
        ModelState.Remove("Module");
        ModelState.Remove("Resources");
        ModelState.Remove("Progresses");
        ModelState.Remove("LessonTranscripts");

        if (!ModelState.IsValid)
        {
            return View(lesson);
        }

        lesson.IsPublished = isPublish;

        await _lessonService.UpdateLessonAsync(lesson);

        return RedirectToAction("Index", new { moduleId = lesson.ModuleId });
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Delete(long id, long moduleId)
    {
        await _lessonService.DeleteLessonAsync(id);

        return RedirectToAction("Index", new { moduleId });
    }
    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> MyLessons()
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var lessons = await _lessonService.GetPublishedLessonsForStudentAsync(studentId);

        return View("MyLessonsList", lessons);
    }
}