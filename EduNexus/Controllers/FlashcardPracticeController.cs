using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Flashcards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Student)]
public class FlashcardPracticeController : Controller
{
    private readonly IFlashcardService _flashcardService;

    public FlashcardPracticeController(
        IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    public async Task<IActionResult> Library(
        string? keyword,
        long? courseId,
        string? sortBy = "newest")
    {
        var model = await _flashcardService.GetFlashcardLibraryAsync(
            keyword,
            courseId,
            sortBy);

        return View(model);
    }

    public async Task<IActionResult> Study(long id)
    {
        var model = await _flashcardService.GetFlashcardPracticeAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Flashcard Practice";

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> SavePractice(
    [FromBody] FlashcardPracticeSubmitViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        long studentId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _flashcardService.SavePracticeAsync(
            studentId,
            model);

        return Ok();
    }
}