using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Student)]
public class SubmissionController : Controller
{
    public IActionResult MyAssignments()
    {
        ViewData["Title"] = "My Assignments";
        ViewData["Message"] = "Assignment submission module is being developed by Person 3.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}