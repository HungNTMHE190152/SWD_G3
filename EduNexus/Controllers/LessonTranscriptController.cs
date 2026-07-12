using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class LessonTranscriptController : Controller
{
    private readonly ILessonTranscriptService _transcriptService;

    public LessonTranscriptController(ILessonTranscriptService transcriptService)
    {
        _transcriptService = transcriptService;
    }

    public async Task<IActionResult> Index(long lessonId)
    {
        var transcript = await _transcriptService.GetByLessonIdAsync(lessonId);
        ViewBag.LessonId = lessonId;
        return View(transcript);
    }

    [HttpPost]
    public async Task<IActionResult> Generate(long lessonId)
    {
        await _transcriptService.GenerateFakeAsync(lessonId);
        return RedirectToAction("Index", new { lessonId });
    }
}