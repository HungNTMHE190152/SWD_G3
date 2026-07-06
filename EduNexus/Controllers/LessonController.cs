using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize]
public class LessonController : Controller
{
    [Authorize(Roles = RoleNames.Sme)]
    public IActionResult Index()
    {
        ViewData["Title"] = "Lesson Editor";
        ViewData["Message"] = "Lesson editor is being developed by Person 2.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }

    [Authorize(Roles = RoleNames.Student)]
    public IActionResult MyLessons()
    {
        ViewData["Title"] = "My Lessons";
        ViewData["Message"] = "Student lesson view is being developed by Person 2.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}