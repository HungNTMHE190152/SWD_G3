using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class ChoiceService : IChoiceService
{
    private readonly EduNexusContext _context;

    public ChoiceService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<Choice>> GetByQuestionAsync(long questionId)
    {
        return await _context.Choices
            .Where(x => x.QuestionId == questionId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Choice?> GetByIdAsync(long id)
    {
        return await _context.Choices
            .Include(x => x.Question)
            .FirstOrDefaultAsync(x => x.ChoiceId == id);
    }

    public async Task<bool> CreateAsync(Choice choice)
    {
        _context.Choices.Add(choice);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Choice choice)
    {
        _context.Choices.Update(choice);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var choice = await _context.Choices.FindAsync(id);

        if (choice == null)
            return false;

        _context.Choices.Remove(choice);

        return await _context.SaveChangesAsync() > 0;
    }
}