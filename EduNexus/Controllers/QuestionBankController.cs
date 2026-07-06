using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class QuestionBankController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Question Bank";
        ViewData["Message"] = "Question bank module is being developed by Person 5.";
        return View("~/Views/Shared/UnderConstruction.cshtml");
    }
}