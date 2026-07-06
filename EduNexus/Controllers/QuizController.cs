using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize]
public class QuizController : Controller
{
    [Authorize(Roles = RoleNames.Sme)]
    public IActionResult Manage()
    {
        ViewData["Title"] = "Quiz Management";
        ViewData["Message"] = "Quiz management module is being developed by Person 5.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }

    [Authorize(Roles = RoleNames.Student)]
    public IActionResult NewQuiz()
    {
        ViewData["Title"] = "New Quiz";
        ViewData["Message"] = "Student quiz module is being developed by Person 5.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }

    [Authorize(Roles = RoleNames.Student)]
    public IActionResult History()
    {
        ViewData["Title"] = "Quiz History";
        ViewData["Message"] = "Quiz history module is being developed by Person 5.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}