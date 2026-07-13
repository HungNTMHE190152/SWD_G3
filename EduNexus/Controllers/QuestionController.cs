using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class QuestionController : Controller
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    public async Task<IActionResult> Create(long bankId)
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _questionService.BuildCreateFormAsync(bankId, smeId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestionFormViewModel model)
    {
        long smeId = User.GetCurrentUserId();

        if (smeId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _questionService.CreateAsync(model, smeId);
            return RedirectToAction("Details", "QuestionBank", new { id = model.BankId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }
}