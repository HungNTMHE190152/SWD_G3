using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize(Roles = RoleNames.Student)]
[AllowAnonymous]
public class QuizReviewController : Controller
{
    private readonly IQuizResultService _resultService;
    private readonly long demoStudentId = 1;

    public QuizReviewController(IQuizResultService resultService)
    {
        _resultService = resultService;
    }

    [HttpGet]
    public async Task<IActionResult> Review(long attemptId)
    {
        var review = await _resultService.GetAttemptReviewAsync(attemptId, demoStudentId);
        if (review == null) return NotFound();
        return View(review);
    }
}
