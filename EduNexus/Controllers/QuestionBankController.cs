using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize(Roles = RoleNames.Sme)]
[AllowAnonymous]
public class QuestionBankController : Controller
{
    private readonly IQuestionBankService _bankService;
    private readonly long demoSmeId = 2;

    public QuestionBankController(IQuestionBankService bankService)
    {
        _bankService = bankService;
    }

    public async Task<IActionResult> Index(long courseId = 1) // Default to course 1 for demo
    {
        var banks = await _bankService.GetBanksByCourseAsync(courseId);
        ViewBag.CourseId = courseId;
        return View(banks);
    }

    [HttpGet]
    public IActionResult Create(long courseId)
    {
        var model = new QuestionBankFormViewModel { CourseId = courseId };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestionBankFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _bankService.CreateBankAsync(model, demoSmeId);
            return RedirectToAction(nameof(Index), new { courseId = model.CourseId });
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var model = await _bankService.GetBankByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(QuestionBankFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _bankService.UpdateBankAsync(model);
            return RedirectToAction(nameof(Index), new { courseId = model.CourseId });
        }
        return View(model);
    }
}