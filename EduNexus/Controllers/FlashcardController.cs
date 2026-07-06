using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class FlashcardController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Flashcard Editor";
        ViewData["Message"] = "Flashcard module is being developed by Person 4.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}