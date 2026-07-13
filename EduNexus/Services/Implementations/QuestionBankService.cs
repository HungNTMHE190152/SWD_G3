using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class QuestionBankService : IQuestionBankService
{
    private readonly EduNexusContext _context;

    public QuestionBankService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<QuestionBankListItemViewModel>> GetBanksForSmeAsync(long smeId)
    {
        var banks = await (
            from bank in _context.QuestionBanks
            join course in _context.Courses on bank.CourseId equals course.CourseId
            where bank.CreatedBy == smeId
            orderby bank.CreatedAt descending
            select new QuestionBankListItemViewModel
            {
                BankId = bank.BankId,
                BankName = bank.BankName,
                Description = bank.Description,
                CourseTitle = course.Title,
                IsPublic = bank.IsPublic ?? false,
                CreatedAt = bank.CreatedAt,
                QuestionCount = _context.Questions.Count(q => q.BankId == bank.BankId)
            }
        ).ToListAsync();

        return banks;
    }

    public async Task<QuestionBankFormViewModel> BuildCreateFormAsync(long smeId)
    {
        var courses = await _context.Courses
            .Where(c => c.CreatedBy == smeId)
            .OrderBy(c => c.Title)
            .ToListAsync();

        if (!courses.Any())
        {
            courses = await _context.Courses
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        return new QuestionBankFormViewModel
        {
            Courses = courses.Select(c => new SelectListItem
            {
                Value = c.CourseId.ToString(),
                Text = $"{c.CourseCode} - {c.Title}"
            }).ToList()
        };
    }

    public async Task<long> CreateAsync(QuestionBankFormViewModel model, long smeId)
    {
        bool courseExists = await _context.Courses
            .AnyAsync(c => c.CourseId == model.CourseId);

        if (!courseExists)
        {
            throw new InvalidOperationException("Selected course does not exist.");
        }

        var bank = new QuestionBank
        {
            CourseId = model.CourseId,
            CreatedBy = smeId,
            BankName = model.BankName.Trim(),
            Description = model.Description,
            IsPublic = model.IsPublic,
            CreatedAt = DateTime.Now
        };

        _context.QuestionBanks.Add(bank);
        await _context.SaveChangesAsync();

        return bank.BankId;
    }

    public async Task<QuestionListViewModel?> GetQuestionListAsync(long bankId, long smeId)
    {
        var bankInfo = await (
            from bank in _context.QuestionBanks
            join course in _context.Courses on bank.CourseId equals course.CourseId
            where bank.BankId == bankId && bank.CreatedBy == smeId
            select new
            {
                bank.BankId,
                bank.BankName,
                CourseTitle = course.Title
            }
        ).FirstOrDefaultAsync();

        if (bankInfo == null)
        {
            return null;
        }

        var questions = await _context.Questions
            .Where(q => q.BankId == bankId)
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new QuestionListItemViewModel
            {
                QuestionId = q.QuestionId,
                BankId = q.BankId,
                QuestionContent = q.QuestionContent,
                QuestionType = q.QuestionType,
                Difficulty = q.Difficulty,
                Explanation = q.Explanation,
                IsAIGenerated = false,
                CreatedAt = q.CreatedAt,
                ChoiceCount = _context.Choices.Count(c => c.QuestionId == q.QuestionId),
                CorrectChoiceCount = _context.Choices.Count(c => c.QuestionId == q.QuestionId && c.IsCorrect == true)
            })
            .ToListAsync();

        return new QuestionListViewModel
        {
            BankId = bankInfo.BankId,
            BankName = bankInfo.BankName,
            CourseTitle = bankInfo.CourseTitle,
            Questions = questions
        };
    }
}