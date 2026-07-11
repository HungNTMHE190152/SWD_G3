using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize]
public class LessonResourceController : Controller
{
    private readonly IResourceService _resourceService;

    public LessonResourceController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Index(long lessonId)
    {
        var resources = await _resourceService.GetByLessonIdAsync(lessonId);
        ViewBag.LessonId = lessonId;
        return View(resources);
    }

    [Authorize(Roles = RoleNames.Student)]
    public async Task<IActionResult> View(long lessonId)
    {
        var resources = await _resourceService.GetByLessonIdAsync(lessonId);
        return View(resources);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Upload(long lessonId, IFormFile file)
    {
        await _resourceService.UploadAsync(lessonId, file);
        return RedirectToAction("Index", new { lessonId });
    }
}