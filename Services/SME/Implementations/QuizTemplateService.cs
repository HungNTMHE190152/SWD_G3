using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class QuizTemplateService : IQuizTemplateService
{
    private readonly EduNexusContext _context;

    public QuizTemplateService(EduNexusContext context)
    {
        _context = context;
    }

    //=========================================
    // LIST
    //=========================================

    public async Task<List<QuizTemplate>> GetAllAsync()
    {
        return await _context.QuizTemplates
            .Include(x => x.Course)
            .Include(x => x.Module)
            .Include(x => x.QuizTemplateQuestions)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    //=========================================
    // DETAILS
    //=========================================

    public async Task<QuizTemplate?> GetByIdAsync(long id)
    {
        return await _context.QuizTemplates
            .Include(x => x.Course)
            .Include(x => x.Module)
            .Include(x => x.QuizTemplateQuestions)
                .ThenInclude(x => x.Question)
            .FirstOrDefaultAsync(x => x.QuizTemplateId == id);
    }

    //=========================================
    // CREATE
    //=========================================

    public async Task<bool> CreateAsync(QuizTemplate quizTemplate)
    {
        quizTemplate.CreatedAt = DateTime.UtcNow;

        _context.QuizTemplates.Add(quizTemplate);

        return await _context.SaveChangesAsync() > 0;
    }

    //=========================================
    // UPDATE
    //=========================================

    public async Task<bool> UpdateAsync(QuizTemplate quizTemplate)
    {
        quizTemplate.UpdatedAt = DateTime.UtcNow;

        _context.QuizTemplates.Update(quizTemplate);

        return await _context.SaveChangesAsync() > 0;
    }

    //=========================================
    // DELETE
    //=========================================

    public async Task<bool> DeleteAsync(long id)
    {
        var quiz = await _context.QuizTemplates.FindAsync(id);

        if (quiz == null)
            return false;

        _context.QuizTemplates.Remove(quiz);

        return await _context.SaveChangesAsync() > 0;
    }

    //=========================================
    // COURSE
    //=========================================

    public async Task<List<Course>> GetCoursesAsync()
    {
        return await _context.Courses
            .OrderBy(x => x.Title)
            .ToListAsync();
    }

    //=========================================
    // MODULE
    //=========================================

    public async Task<List<Module>> GetModulesAsync(long courseId)
    {
        return await _context.Modules
            .Where(x => x.CourseId == courseId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    //=========================================
    // QUESTIONS
    //=========================================

    public async Task<List<Question>> GetQuestionsAsync(long courseId)
    {
        return await _context.Questions
            .Include(x => x.QuestionBank)
            .Where(x => x.QuestionBank.CourseId == courseId)
            .OrderBy(x => x.QuestionContent)
            .ToListAsync();
    }
}