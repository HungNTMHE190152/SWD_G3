using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize(Roles = RoleNames.Sme)]
[AllowAnonymous]
public class AiQuestionController : Controller
{
    private readonly IQuestionService _questionService;
    private readonly long demoSmeId = 2; // Fixed ID for now

    public AiQuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public IActionResult Index(long bankId)
    {
        ViewBag.BankId = bankId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Generate(long bankId, string topic)
    {
        ViewBag.BankId = bankId;
        ViewBag.Topic = topic;

        // Faking the AI Question generation based on the topic
        var mockQuestions = new List<QuestionFormViewModel>
        {
            new QuestionFormViewModel
            {
                BankId = bankId,
                QuestionContent = $"What is a fundamental concept related to {topic}?",
                QuestionType = "Single Choice",
                Difficulty = "Medium",
                Explanation = "Generated automatically by AI simulation based on the prompt.",
                IsAigenerated = true,
                Choices = new List<ChoiceFormViewModel>
                {
                    new ChoiceFormViewModel { ChoiceContent = "The main core of " + topic, IsCorrect = true, DisplayOrder = 1 },
                    new ChoiceFormViewModel { ChoiceContent = "An unrelated distractor", IsCorrect = false, DisplayOrder = 2 },
                    new ChoiceFormViewModel { ChoiceContent = "A common misconception", IsCorrect = false, DisplayOrder = 3 }
                }
            },
            new QuestionFormViewModel
            {
                BankId = bankId,
                QuestionContent = $"Is {topic} widely accepted in the industry as a standard?",
                QuestionType = "True/False",
                Difficulty = "Easy",
                Explanation = "Generated automatically by AI simulation.",
                IsAigenerated = true,
                Choices = new List<ChoiceFormViewModel>
                {
                    new ChoiceFormViewModel { ChoiceContent = "True", IsCorrect = true, DisplayOrder = 1 },
                    new ChoiceFormViewModel { ChoiceContent = "False", IsCorrect = false, DisplayOrder = 2 }
                }
            }
        };

        return View("Staging", mockQuestions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(long bankId, string topic)
    {
        // Re-create the mocked list and save them because we don't have a temporary DB table.
        // In a real app, this would read from the actual Staging table.
        var mockQuestions = new List<QuestionFormViewModel>
        {
            new QuestionFormViewModel
            {
                BankId = bankId,
                QuestionContent = $"What is a fundamental concept related to {topic}?",
                QuestionType = "Single Choice",
                Difficulty = "Medium",
                Explanation = "Generated automatically by AI simulation based on the prompt.",
                IsAigenerated = true,
                Choices = new List<ChoiceFormViewModel>
                {
                    new ChoiceFormViewModel { ChoiceContent = "The main core of " + topic, IsCorrect = true, DisplayOrder = 1 },
                    new ChoiceFormViewModel { ChoiceContent = "An unrelated distractor", IsCorrect = false, DisplayOrder = 2 },
                    new ChoiceFormViewModel { ChoiceContent = "A common misconception", IsCorrect = false, DisplayOrder = 3 }
                }
            },
            new QuestionFormViewModel
            {
                BankId = bankId,
                QuestionContent = $"Is {topic} widely accepted in the industry as a standard?",
                QuestionType = "True/False",
                Difficulty = "Easy",
                Explanation = "Generated automatically by AI simulation.",
                IsAigenerated = true,
                Choices = new List<ChoiceFormViewModel>
                {
                    new ChoiceFormViewModel { ChoiceContent = "True", IsCorrect = true, DisplayOrder = 1 },
                    new ChoiceFormViewModel { ChoiceContent = "False", IsCorrect = false, DisplayOrder = 2 }
                }
            }
        };

        foreach (var q in mockQuestions)
        {
            await _questionService.CreateQuestionAsync(q, demoSmeId);
        }

        return RedirectToAction("Index", "Question", new { bankId = bankId });
    }
}
