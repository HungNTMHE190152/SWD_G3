using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class QuestionService : IQuestionService
{
    private readonly EduNexusContext _context;

    public QuestionService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<QuestionFormViewModel?> BuildCreateFormAsync(long bankId, long smeId)
    {
        var bank = await _context.QuestionBanks
            .FirstOrDefaultAsync(b => b.BankId == bankId && b.CreatedBy == smeId);

        if (bank == null)
        {
            return null;
        }

        return new QuestionFormViewModel
        {
            BankId = bank.BankId,
            BankName = bank.BankName,
            QuestionType = "MULTIPLE_CHOICE",
            Difficulty = "EASY"
        };
    }

    public async Task<long> CreateAsync(QuestionFormViewModel model, long smeId)
    {
        List<ChoiceFormViewModel> validChoices;

        if (model.QuestionType == "TRUE_FALSE")
        {
            bool trueIsCorrect = model.Choices.ElementAtOrDefault(0)?.IsCorrect == true;
            bool falseIsCorrect = model.Choices.ElementAtOrDefault(1)?.IsCorrect == true;

            if (trueIsCorrect == falseIsCorrect)
            {
                throw new InvalidOperationException("True/False question must have exactly one correct answer.");
            }

            validChoices = new List<ChoiceFormViewModel>
    {
        new ChoiceFormViewModel
        {
            ChoiceContent = "True",
            IsCorrect = trueIsCorrect,
            DisplayOrder = 1
        },
        new ChoiceFormViewModel
        {
            ChoiceContent = "False",
            IsCorrect = falseIsCorrect,
            DisplayOrder = 2
        }
    };
        }
        else
        {
            validChoices = model.Choices
                .Where(c => !string.IsNullOrWhiteSpace(c.ChoiceContent))
                .ToList();

            if (validChoices.Count < 2)
            {
                throw new InvalidOperationException("A multiple choice question must have at least 2 choices.");
            }

            int correctCount = validChoices.Count(c => c.IsCorrect);

            if (correctCount != 1)
            {
                throw new InvalidOperationException("Multiple choice question must have exactly one correct answer.");
            }
        }

        var question = new Question
        {
            BankId = model.BankId,
            CreatedBy = smeId,
            QuestionContent = model.QuestionContent.Trim(),
            QuestionType = model.QuestionType,
            Difficulty = model.Difficulty,
            Explanation = model.Explanation,
            CreatedAt = DateTime.Now
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        foreach (var choiceModel in validChoices)
        {
            var choice = new Choice
            {
                QuestionId = question.QuestionId,
                ChoiceContent = choiceModel.ChoiceContent.Trim(),
                IsCorrect = choiceModel.IsCorrect,
                DisplayOrder = choiceModel.DisplayOrder
            };

            _context.Choices.Add(choice);
        }

        await _context.SaveChangesAsync();

        return question.QuestionId;
    }
}