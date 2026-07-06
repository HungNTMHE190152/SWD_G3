using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class CourseController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Course List";
        ViewData["Message"] = "Course module is being developed by Person 2.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }

    public IActionResult Structure(long id)
    {
        ViewData["Title"] = "Course Structure";
        ViewData["Message"] = "Course structure module is being developed by Person 2.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}