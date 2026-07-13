using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class QuestionBankController : Controller
{
    private readonly IQuestionBankService _questionBankService;

    public QuestionBankController(IQuestionBankService questionBankService)
    {
        _questionBankService = questionBankService;
    }

    public async Task<IActionResult> Index()
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var banks = await _questionBankService.GetBanksForSmeAsync(smeId);
        return View(banks);
    }

    public async Task<IActionResult> Create()
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _questionBankService.BuildCreateFormAsync(smeId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestionBankFormViewModel model)
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            var form = await _questionBankService.BuildCreateFormAsync(smeId);
            model.Courses = form.Courses;
            return View(model);
        }

        try
        {
            long bankId = await _questionBankService.CreateAsync(model, smeId);
            return RedirectToAction("Details", new { id = bankId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            var form = await _questionBankService.BuildCreateFormAsync(smeId);
            model.Courses = form.Courses;

            return View(model);
        }
    }

    public async Task<IActionResult> Details(long id)
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _questionBankService.GetQuestionListAsync(id, smeId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }
}