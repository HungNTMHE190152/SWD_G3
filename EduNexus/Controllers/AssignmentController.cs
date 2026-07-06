using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class AssignmentController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Assignment Management";
        ViewData["Message"] = "Assignment module is being developed by Person 3.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}