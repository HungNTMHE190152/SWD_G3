using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class ModuleController : Controller
{
    private readonly IModuleService _moduleService;

    public ModuleController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    //==========================
    // Index
    //==========================

    public async Task<IActionResult> Index()
    {
        var modules = await _moduleService.GetAllAsync();

        return View(modules);
    }

    //==========================
    // Details
    //==========================

    public async Task<IActionResult> Details(long id)
    {
        var module = await _moduleService.GetByIdAsync(id);

        if (module == null)
            return NotFound();

        return View(module);
    }

    //==========================
    // Create
    //==========================

    [HttpGet]
    public async Task<IActionResult> Create(long? courseId)
    {
        await LoadCourses();

        if (courseId != null)
            ViewBag.SelectedCourse = courseId;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ModuleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCourses();

            return View(model);
        }

        Module module = new()
        {
            CourseId = model.CourseId,

            Title = model.Title,

            Description = model.Description,

            DisplayOrder = model.DisplayOrder
        };

        bool result = await _moduleService.CreateAsync(module);

        if (result)
        {
            TempData["Success"] = "Module created successfully.";

            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = "Create failed.";

        await LoadCourses();

        return View(model);
    }

    //==========================
    // Edit
    //==========================

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var module = await _moduleService.GetByIdAsync(id);

        if (module == null)
            return NotFound();

        ModuleViewModel vm = new()
        {
            ModuleId = module.ModuleId,

            CourseId = module.CourseId,

            Title = module.Title,

            Description = module.Description,

            DisplayOrder = module.DisplayOrder
        };

        await LoadCourses();

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ModuleViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await LoadCourses();

            return View(vm);
        }

        var module = await _moduleService.GetByIdAsync(vm.ModuleId);

        if (module == null)
            return NotFound();

        module.CourseId = vm.CourseId;

        module.Title = vm.Title;

        module.Description = vm.Description;

        module.DisplayOrder = vm.DisplayOrder;

        bool result = await _moduleService.UpdateAsync(module);

        if (result)
        {
            TempData["Success"] = "Updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = "Update failed.";

        await LoadCourses();

        return View(vm);
    }

    //==========================
    // Delete
    //==========================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        bool result = await _moduleService.DeleteAsync(id);

        if (result)
            TempData["Success"] = "Deleted.";

        else
            TempData["Error"] = "Delete failed.";

        return RedirectToAction(nameof(Index));
    }

    //==========================
    // Helper
    //==========================

    private async Task LoadCourses()
    {
        var courses = await _moduleService.GetCoursesAsync();

        ViewBag.Courses = new SelectList(
            courses,
            "CourseId",
            "Title");
    }
}