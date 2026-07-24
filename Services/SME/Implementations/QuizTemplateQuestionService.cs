using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class QuizTemplateQuestionService : IQuizTemplateQuestionService
{
    private readonly EduNexusContext _context;

    public QuizTemplateQuestionService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<QuizTemplateQuestion>> GetByQuizAsync(long quizTemplateId)
    {
        return await _context.QuizTemplateQuestions
            .Include(x => x.Question)
            .Where(x => x.QuizTemplateId == quizTemplateId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<QuizTemplateQuestion?> GetByIdAsync(long id)
    {
        return await _context.QuizTemplateQuestions
            .Include(x => x.Question)
            .Include(x => x.QuizTemplate)
            .FirstOrDefaultAsync(x => x.QuizTemplateQuestionId == id);
    }

    public async Task<bool> CreateAsync(QuizTemplateQuestion question)
    {
        _context.QuizTemplateQuestions.Add(question);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.QuizTemplateQuestions.FindAsync(id);

        if (entity == null)
            return false;

        _context.QuizTemplateQuestions.Remove(entity);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Question>> GetAvailableQuestionsAsync(long quizTemplateId)
    {
        var quiz = await _context.QuizTemplates
            .FirstAsync(x => x.QuizTemplateId == quizTemplateId);

        var usedQuestionIds = await _context.QuizTemplateQuestions
            .Where(x => x.QuizTemplateId == quizTemplateId)
            .Select(x => x.QuestionId)
            .ToListAsync();

        return await _context.Questions
            .Include(x => x.QuestionBank)
            .Where(x =>
                x.QuestionBank.CourseId == quiz.CourseId &&
                !usedQuestionIds.Contains(x.QuestionId))
            .OrderBy(x => x.QuestionContent)
            .ToListAsync();
    }
}