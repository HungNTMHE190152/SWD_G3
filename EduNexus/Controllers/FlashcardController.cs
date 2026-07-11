using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Flashcards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize]
public class FlashcardController : Controller
{
    private readonly IFlashcardService _flashcardService;

    public FlashcardController(IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Index()
    {
        long userId = User.GetCurrentUserId();

        if (userId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _flashcardService.GetMyFlashcardSetsAsync(userId);

        return View(model);
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> CreateSet()
    {
        var model = await _flashcardService.GetCreateSetViewModelAsync();

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSet(FlashcardSetFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var reloadModel = await _flashcardService.GetCreateSetViewModelAsync();

            model.Courses = reloadModel.Courses;

            return View(model);
        }

        long userId = User.GetCurrentUserId();

        await _flashcardService.CreateFlashcardSetAsync(userId, model);

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> EditSet(long id)
    {
        var model = await _flashcardService.GetEditSetViewModelAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSet(FlashcardSetFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Courses = (await _flashcardService
                .GetCreateSetViewModelAsync()).Courses;

            return View(model);
        }

        long userId = User.GetCurrentUserId();

        await _flashcardService.UpdateFlashcardSetAsync(userId, model);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSet(long flashcardSetId)
    {
        await _flashcardService.DeleteFlashcardSetAsync(flashcardSetId);

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Details(long id)
    {
        var model = await _flashcardService.GetFlashcardEditorAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Staging(long id)
    {
        var model = await _flashcardService.GetFlashcardStagingAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    // Nếu chưa dùng Preview thì có thể xóa.
    // Nếu đã tạo View Preview.cshtml thì giữ lại.
    [Authorize(Roles = RoleNames.Sme)]
    public async Task<IActionResult> Preview(long id)
    {
        var model = await _flashcardService.GetFlashcardPreviewAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFlashcard(FlashcardFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Details),
                new { id = model.FlashcardSetId });
        }

        await _flashcardService.CreateFlashcardAsync(model);

        return RedirectToAction(nameof(Details),
            new { id = model.FlashcardSetId });
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateFlashcard(FlashcardFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Details),
                new { id = model.FlashcardSetId });
        }

        await _flashcardService.UpdateFlashcardAsync(model);

        return RedirectToAction(nameof(Details),
            new { id = model.FlashcardSetId });
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Sme)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFlashcard(long flashcardId, long flashcardSetId)
    {
        await _flashcardService.DeleteFlashcardAsync(flashcardId);

        return RedirectToAction(nameof(Details),
            new { id = flashcardSetId });
    }
}