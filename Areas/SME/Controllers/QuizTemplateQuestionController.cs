using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class QuizTemplateQuestionController : Controller
{
    private readonly IQuizTemplateQuestionService _service;
    private readonly IQuizTemplateService _quizService;

    public QuizTemplateQuestionController(
        IQuizTemplateQuestionService service,
        IQuizTemplateService quizService)
    {
        _service = service;
        _quizService = quizService;
    }

    //=========================================
    // LIST
    //=========================================

    public async Task<IActionResult> Index(long quizTemplateId)
    {
        ViewBag.QuizTemplateId = quizTemplateId;

        var questions =
            await _service.GetByQuizAsync(quizTemplateId);

        return View(questions);
    }

    //=========================================
    // CREATE
    //=========================================

    [HttpGet]
    public async Task<IActionResult> Create(long quizTemplateId)
    {
        var questions =
            await _service.GetAvailableQuestionsAsync(quizTemplateId);

        ViewBag.Questions = new SelectList(
            questions,
            "QuestionId",
            "QuestionContent");

        ViewBag.QuizTemplateId = quizTemplateId;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        long quizTemplateId,
        long questionId,
        decimal score,
        int displayOrder)
    {
        QuizTemplateQuestion entity = new()
        {
            QuizTemplateId = quizTemplateId,
            QuestionId = questionId,
            Score = score,
            DisplayOrder = displayOrder
        };

        await _service.CreateAsync(entity);

        return RedirectToAction(
            nameof(Index),
            new
            {
                quizTemplateId
            });
    }

    //=========================================
    // DELETE
    //=========================================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        var entity =
            await _service.GetByIdAsync(id);

        if (entity == null)
            return NotFound();

        long quizId = entity.QuizTemplateId;

        await _service.DeleteAsync(id);

        return RedirectToAction(
            nameof(Index),
            new
            {
                quizTemplateId = quizId
            });
    }
}