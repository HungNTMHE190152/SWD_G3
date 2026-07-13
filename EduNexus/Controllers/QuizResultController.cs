using EduNexus.Constants;
using EduNexus.Helpers;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Student)]
public class QuizResultController : Controller
{
    private readonly IQuizAttemptService _quizAttemptService;

    public QuizResultController(IQuizAttemptService quizAttemptService)
    {
        _quizAttemptService = quizAttemptService;
    }

    public async Task<IActionResult> Result(long id)
    {
        long studentId = User.GetCurrentUserId();

        if (studentId == 0)
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = await _quizAttemptService.GetResultAsync(id, studentId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }
}