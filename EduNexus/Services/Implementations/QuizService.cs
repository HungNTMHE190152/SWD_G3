using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Quizzes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class QuizService : IQuizService
{
    private readonly EduNexusContext _context;

    public QuizService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<QuizListItemViewModel>> GetQuizzesForSmeAsync(long smeId)
    {
        var quizzes = await (
            from quiz in _context.Quizzes
            join assignment in _context.Assignments
                on quiz.AssignmentId equals assignment.AssignmentId
            join module in _context.Modules
                on assignment.ModuleId equals module.ModuleId
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where quiz.CreatedBy == smeId
            orderby quiz.CreatedAt descending
            select new QuizListItemViewModel
            {
                QuizId = quiz.QuizId,
                AssignmentId = quiz.AssignmentId,
                QuizTitle = quiz.QuizTitle,
                Description = quiz.Description,
                CourseTitle = course.Title,
                ModuleName = module.ModuleName,
                Duration = quiz.Duration,
                PassingScore = quiz.PassingScore,
                ShuffleQuestion = quiz.ShuffleQuestion == true,
                ShuffleAnswer = quiz.ShuffleAnswer == true,
                CreatedAt = quiz.CreatedAt,
                QuestionCount = _context.QuizQuestions.Count(qq => qq.QuizId == quiz.QuizId)
            }
        ).ToListAsync();

        return quizzes;
    }

    public async Task<QuizFormViewModel> BuildCreateFormAsync(long smeId)
    {
        var modules = await (
            from module in _context.Modules
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where course.CreatedBy == smeId
            orderby course.Title, module.DisplayOrder
            select new
            {
                module.ModuleId,
                module.ModuleName,
                CourseCode = course.CourseCode,
                CourseTitle = course.Title
            }
        ).ToListAsync();

        if (!modules.Any())
        {
            modules = await (
                from module in _context.Modules
                join course in _context.Courses
                    on module.CourseId equals course.CourseId
                orderby course.Title, module.DisplayOrder
                select new
                {
                    module.ModuleId,
                    module.ModuleName,
                    CourseCode = course.CourseCode,
                    CourseTitle = course.Title
                }
            ).ToListAsync();
        }

        return new QuizFormViewModel
        {
            Modules = modules.Select(m => new SelectListItem
            {
                Value = m.ModuleId.ToString(),
                Text = $"{m.CourseCode} - {m.CourseTitle} / {m.ModuleName}"
            }).ToList()
        };
    }

    public async Task<long> CreateQuizAsync(QuizFormViewModel model, long smeId)
    {
        var moduleInfo = await (
            from module in _context.Modules
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where module.ModuleId == model.ModuleId
            select new
            {
                module.ModuleId,
                module.ModuleName,
                CourseId = course.CourseId,
                CourseTitle = course.Title
            }
        ).FirstOrDefaultAsync();

        if (moduleInfo == null)
        {
            throw new InvalidOperationException("Selected module does not exist.");
        }

        var assignment = new Assignment
        {
            ModuleId = model.ModuleId,
            CreatedBy = smeId,
            Title = model.QuizTitle.Trim(),
            Description = model.Description,
            AssignmentType = "QUIZ",
            TotalScore = 10,
            OpenDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            AllowLateSubmission = false,
            Status = "PUBLISHED",
            CreatedAt = DateTime.Now
        };

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();

        var quiz = new Quiz
        {
            AssignmentId = assignment.AssignmentId,
            QuizTitle = model.QuizTitle.Trim(),
            Description = model.Description,
            Duration = model.Duration,
            PassingScore = model.PassingScore,
            ShuffleQuestion = model.ShuffleQuestion,
            ShuffleAnswer = model.ShuffleAnswer,
            CreatedBy = smeId,
            CreatedAt = DateTime.Now
        };

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();

        return quiz.QuizId;
    }

    public async Task<QuizQuestionSelectionViewModel?> BuildQuestionSelectionAsync(
        long quizId,
        long smeId,
        long? bankId)
    {
        var quizInfo = await (
            from quiz in _context.Quizzes
            join assignment in _context.Assignments
                on quiz.AssignmentId equals assignment.AssignmentId
            join module in _context.Modules
                on assignment.ModuleId equals module.ModuleId
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where quiz.QuizId == quizId && quiz.CreatedBy == smeId
            select new
            {
                quiz.QuizId,
                quiz.QuizTitle,
                CourseId = course.CourseId,
                CourseTitle = course.Title
            }
        ).FirstOrDefaultAsync();

        if (quizInfo == null)
        {
            return null;
        }

        var banks = await _context.QuestionBanks
            .Where(b => b.CourseId == quizInfo.CourseId && (b.CreatedBy == smeId || b.IsPublic == true))
            .OrderBy(b => b.BankName)
            .Select(b => new SelectListItem
            {
                Value = b.BankId.ToString(),
                Text = b.BankName
            })
            .ToListAsync();

        long? selectedBankId = bankId;

        if (selectedBankId == null && banks.Any())
        {
            selectedBankId = long.Parse(banks.First().Value);
        }

        var existingQuizQuestions = await _context.QuizQuestions
            .Where(qq => qq.QuizId == quizId)
            .ToListAsync();

        var existingMap = existingQuizQuestions
            .ToDictionary(x => x.QuestionId, x => x);

        var model = new QuizQuestionSelectionViewModel
        {
            QuizId = quizInfo.QuizId,
            QuizTitle = quizInfo.QuizTitle,
            CourseTitle = quizInfo.CourseTitle,
            BankId = selectedBankId,
            QuestionBanks = banks
        };

        if (selectedBankId == null)
        {
            return model;
        }

        var questions = await _context.Questions
            .Where(q => q.BankId == selectedBankId.Value)
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new
            {
                q.QuestionId,
                q.QuestionContent,
                q.QuestionType,
                q.Difficulty,
                ChoiceCount = _context.Choices.Count(c => c.QuestionId == q.QuestionId)
            })
            .ToListAsync();

        int defaultOrder = 1;

        foreach (var question in questions)
        {
            bool alreadyAdded = existingMap.ContainsKey(question.QuestionId);

            model.Questions.Add(new QuizQuestionItemViewModel
            {
                QuestionId = question.QuestionId,
                QuestionContent = question.QuestionContent,
                QuestionType = question.QuestionType,
                Difficulty = question.Difficulty,
                ChoiceCount = question.ChoiceCount,
                IsSelected = alreadyAdded,
                Score = alreadyAdded ? existingMap[question.QuestionId].Score ?? 1 : 1,
                DisplayOrder = alreadyAdded ? existingMap[question.QuestionId].DisplayOrder ?? defaultOrder : defaultOrder
            });

            defaultOrder++;
        }

        return model;
    }

    public async Task UpdateQuizQuestionsAsync(
        QuizQuestionSelectionViewModel model,
        long smeId)
    {
        var quizInfo = await (
            from quiz in _context.Quizzes
            join assignment in _context.Assignments
                on quiz.AssignmentId equals assignment.AssignmentId
            join module in _context.Modules
                on assignment.ModuleId equals module.ModuleId
            join course in _context.Courses
                on module.CourseId equals course.CourseId
            where quiz.QuizId == model.QuizId && quiz.CreatedBy == smeId
            select new
            {
                quiz.QuizId,
                CourseId = course.CourseId
            }
        ).FirstOrDefaultAsync();

        if (quizInfo == null)
        {
            throw new InvalidOperationException("Quiz not found.");
        }

        var selectedQuestions = model.Questions
            .Where(q => q.IsSelected)
            .GroupBy(q => q.QuestionId)
            .Select(g => g.First())
            .ToList();

        if (!selectedQuestions.Any())
        {
            throw new InvalidOperationException("Please select at least one question.");
        }

        var selectedQuestionIds = selectedQuestions
            .Select(q => q.QuestionId)
            .ToList();

        var validQuestionIds = await (
            from question in _context.Questions
            join bank in _context.QuestionBanks
                on question.BankId equals bank.BankId
            where bank.CourseId == quizInfo.CourseId
                  && selectedQuestionIds.Contains(question.QuestionId)
            select question.QuestionId
        ).ToListAsync();

        if (validQuestionIds.Count != selectedQuestionIds.Count)
        {
            throw new InvalidOperationException("Some selected questions do not belong to this quiz course.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        var oldQuestions = await _context.QuizQuestions
            .Where(qq => qq.QuizId == model.QuizId)
            .ToListAsync();

        _context.QuizQuestions.RemoveRange(oldQuestions);
        await _context.SaveChangesAsync();

        int order = 1;

        foreach (var item in selectedQuestions)
        {
            var quizQuestion = new QuizQuestion
            {
                QuizId = model.QuizId,
                QuestionId = item.QuestionId,
                Score = item.Score <= 0 ? 1 : item.Score,
                DisplayOrder = order
            };

            _context.QuizQuestions.Add(quizQuestion);
            order++;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}