using EduNexus.Constants;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class AiLessonController : Controller
{
    private readonly IAiLessonService _aiLessonService;
    private readonly ILessonService _lessonService;

    public AiLessonController(
        IAiLessonService aiLessonService,
        ILessonService lessonService)
    {
        _aiLessonService = aiLessonService;
        _lessonService = lessonService;
    }

    public IActionResult Staging(long moduleId)
    {
        ViewBag.ModuleId = moduleId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Generate(long moduleId, string prompt)
    {
        var lesson = await _aiLessonService.GenerateFakeLessonAsync(moduleId, prompt);
        return View("Preview", lesson);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(Lesson lesson)
    {
        ModelState.Remove("Module");
        ModelState.Remove("Resources");
        ModelState.Remove("Progresses");
        ModelState.Remove("LessonTranscripts");

        lesson.IsPublished = false;
        lesson.CreatedAt = DateTime.Now;

        await _lessonService.CreateLessonAsync(lesson);

        return RedirectToAction("Index", "Lesson", new { moduleId = lesson.ModuleId });
    }
}