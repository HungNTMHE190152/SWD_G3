using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Student)]
public class FlashcardPracticeController : Controller
{
    public IActionResult Library()
    {
        ViewData["Title"] = "Flashcard Library";
        ViewData["Message"] = "Flashcard practice module is being developed by Person 4.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}