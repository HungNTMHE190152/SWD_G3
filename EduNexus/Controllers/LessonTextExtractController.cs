using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class LessonTextExtractController : Controller
{
    private readonly ILessonService _lessonService;
    private readonly ILessonTranscriptService _transcriptService;

    public LessonTextExtractController(
        ILessonService lessonService,
        ILessonTranscriptService transcriptService)
    {
        _lessonService = lessonService;
        _transcriptService = transcriptService;
    }

    public async Task<IActionResult> Index(long lessonId)
    {
        var lesson = await _lessonService.GetLessonDetailAsync(lessonId);
        var transcript = await _transcriptService.GetByLessonIdAsync(lessonId);

        ViewBag.Lesson = lesson;
        ViewBag.Transcript = transcript;

        return View();
    }
}