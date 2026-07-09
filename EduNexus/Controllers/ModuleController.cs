using EduNexus.Constants;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class ModuleController : Controller
{
    private readonly IModuleService _moduleService;

    public ModuleController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    public IActionResult Create(long courseId)
    {
        var module = new Module
        {
            CourseId = courseId,
            DisplayOrder = 1,
            IsPublished = false
        };

        return View(module);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Module module)
    {
        ModelState.Remove("Course");
        ModelState.Remove("Assignments");
        ModelState.Remove("Lessons");

        if (!ModelState.IsValid)
        {
            return View(module);
        }

        module.CreatedAt = DateTime.Now;

        await _moduleService.CreateModuleAsync(module);

        return RedirectToAction("Structure", "Course", new { id = module.CourseId });
    }
}