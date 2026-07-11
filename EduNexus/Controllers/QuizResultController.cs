using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize(Roles = RoleNames.Student)]
[AllowAnonymous]
public class QuizResultController : Controller
{
    private readonly IQuizResultService _resultService;
    private readonly long demoStudentId = 1;

    public QuizResultController(IQuizResultService resultService)
    {
        _resultService = resultService;
    }

    [HttpGet]
    public async Task<IActionResult> History(long quizId)
    {
        var history = await _resultService.GetStudentQuizHistoryAsync(quizId, demoStudentId);
        return View(history);
    }

    [HttpGet]
    public async Task<IActionResult> Result(long attemptId)
    {
        var result = await _resultService.GetAttemptResultAsync(attemptId, demoStudentId);
        if (result == null) return NotFound();
        return View(result);
    }
}
