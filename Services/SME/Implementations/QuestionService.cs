using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class QuestionService : IQuestionService
{
    private readonly EduNexusContext _context;

    public QuestionService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<Question>> GetByBankAsync(long bankId)
    {
        return await _context.Questions
            .Include(x => x.Choices)
            .Where(x => x.QuestionBankId == bankId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Question?> GetByIdAsync(long id)
    {
        return await _context.Questions
            .Include(x => x.Choices)
            .Include(x => x.QuestionBank)
            .FirstOrDefaultAsync(x => x.QuestionId == id);
    }

    public async Task<bool> CreateAsync(Question question)
    {
        question.CreatedAt = DateTime.UtcNow;
        question.CreatedBy = 1;

        _context.Questions.Add(question);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Question question)
    {
        question.UpdatedAt = DateTime.UtcNow;

        _context.Questions.Update(question);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var question = await _context.Questions.FindAsync(id);

        if (question == null)
            return false;

        _context.Questions.Remove(question);

        return await _context.SaveChangesAsync() > 0;
    }
}