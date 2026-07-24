using EduNexus.Models;
using EduNexus.Services.SME.Implementations;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.Lesson;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class LessonResourceController : Controller
{
    private readonly ILessonResourceService _resourceService;
    private readonly IWebHostEnvironment _environment;

    public LessonResourceController(
        ILessonResourceService resourceService,
        IWebHostEnvironment environment)
    {
        _resourceService = resourceService;
        _environment = environment;
    }

    //=====================================
    // INDEX
    //=====================================

    public async Task<IActionResult> Index(long lessonId)
    {
        ViewBag.LessonId = lessonId;

        var resources = await _resourceService.GetByLessonAsync(lessonId);

        return View(resources);
    }

    //=====================================
    // CREATE
    //=====================================

    [HttpGet]
    public IActionResult Create(long lessonId)
    {
        return View(new LessonResourceViewModel
        {
            LessonId = lessonId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LessonResourceViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        string? resourceUrl = vm.ResourceUrl;

        // Upload file
        if (vm.ResourceType == "FILE")
        {
            if (vm.UploadFile == null)
            {
                ModelState.AddModelError("", "Please choose a file.");

                return View(vm);
            }

            string extension =
                Path.GetExtension(vm.UploadFile.FileName).ToLower();

            string folder = "documents";

            if (new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" }.Contains(extension))
            {
                folder = "images";
            }
            else if (new[] { ".mp4", ".mov", ".avi", ".wmv", ".mkv" }.Contains(extension))
            {
                folder = "videos";
            }

            string uploadFolder =
                Path.Combine(_environment.WebRootPath,
                    "uploads",
                    folder);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string fileName =
                Guid.NewGuid() +
                extension;

            string savePath =
                Path.Combine(uploadFolder, fileName);

            using (var stream =
                   new FileStream(savePath, FileMode.Create))
            {
                await vm.UploadFile.CopyToAsync(stream);
            }

            resourceUrl =
                $"/uploads/{folder}/{fileName}";
        }

        LessonResource resource = new()
        {
            LessonId = vm.LessonId,
            ResourceName = vm.ResourceName,
            ResourceType = vm.ResourceType,
            ResourceUrl = resourceUrl!,
            DisplayOrder = vm.DisplayOrder,
            CreatedAt = DateTime.Now
        };

        await _resourceService.CreateAsync(resource);

        TempData["Success"] = "Upload successful.";

        return RedirectToAction(nameof(Index),
            new
            {
                lessonId = vm.LessonId
            });
    }

    //=====================================
    // DETAILS
    //=====================================

    public async Task<IActionResult> Details(long id)
    {
        var resource = await _resourceService.GetByIdAsync(id);

        if (resource == null)
            return NotFound();

        return View(resource);
    }

    //=====================================
    // EDIT
    //=====================================

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var resource =
            await _resourceService.GetByIdAsync(id);

        if (resource == null)
            return NotFound();

        LessonResourceViewModel vm = new()
        {
            LessonResourceId = resource.LessonResourceId,
            LessonId = resource.LessonId,
            ResourceName = resource.ResourceName,
            ResourceType = resource.ResourceType,
            ResourceUrl = resource.ResourceUrl,
            DisplayOrder = resource.DisplayOrder
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LessonResourceViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var resource =
            await _resourceService.GetByIdAsync(vm.LessonResourceId);

        if (resource == null)
            return NotFound();

        resource.ResourceName = vm.ResourceName;
        resource.DisplayOrder = vm.DisplayOrder;
        resource.ResourceType = vm.ResourceType;

        if (vm.ResourceType == "LINK")
        {
            resource.ResourceUrl = vm.ResourceUrl;
        }

        if (vm.ResourceType == "FILE" &&
            vm.UploadFile != null)
        {
            string extension =
                Path.GetExtension(vm.UploadFile.FileName).ToLower();

            string folder = "documents";

            if (new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" }.Contains(extension))
            {
                folder = "images";
            }
            else if (new[] { ".mp4", ".mov", ".avi", ".wmv", ".mkv" }.Contains(extension))
            {
                folder = "videos";
            }

            string uploadFolder =
                Path.Combine(_environment.WebRootPath,
                    "uploads",
                    folder);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string fileName =
                Guid.NewGuid() +
                extension;

            string savePath =
                Path.Combine(uploadFolder, fileName);

            using (var stream =
                   new FileStream(savePath, FileMode.Create))
            {
                await vm.UploadFile.CopyToAsync(stream);
            }

            resource.ResourceUrl =
                $"/uploads/{folder}/{fileName}";
        }

        await _resourceService.UpdateAsync(resource);

        TempData["Success"] = "Updated successfully.";

        return RedirectToAction(nameof(Index),
            new
            {
                lessonId = resource.LessonId
            });
    }

    //=====================================
    // DELETE
    //=====================================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        var resource =
            await _resourceService.GetByIdAsync(id);

        if (resource == null)
            return NotFound();

        long lessonId = resource.LessonId;

        await _resourceService.DeleteAsync(id);

        return RedirectToAction(nameof(Index),
            new
            {
                lessonId
            });
    }
    
   
}