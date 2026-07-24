using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class QuestionBankService : IQuestionBankService
{
    private readonly EduNexusContext _context;

    public QuestionBankService(EduNexusContext context)
    {
        _context = context;
    }

    //==============================
    // GET ALL
    //==============================

    public async Task<List<QuestionBank>> GetAllAsync()
    {
        return await _context.QuestionBanks
            .Include(x => x.Course)
            .Include(x => x.Questions)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    //==============================
    // GET BY ID
    //==============================

    public async Task<QuestionBank?> GetByIdAsync(long id)
    {
        return await _context.QuestionBanks
            .Include(x => x.Course)
            .Include(x => x.Questions)
            .FirstOrDefaultAsync(x => x.QuestionBankId == id);
    }

    //==============================
    // GET COURSES
    //==============================

    public async Task<List<Course>> GetCoursesAsync()
    {
        return await _context.Courses
            .OrderBy(x => x.Title)
            .ToListAsync();
    }

    //==============================
    // CREATE
    //==============================

    public async Task<bool> CreateAsync(QuestionBank bank)
    {
        bank.CreatedAt = DateTime.UtcNow;

        _context.QuestionBanks.Add(bank);

        return await _context.SaveChangesAsync() > 0;
    }

    //==============================
    // UPDATE
    //==============================

    public async Task<bool> UpdateAsync(QuestionBank bank)
    {
        bank.UpdatedAt = DateTime.UtcNow;

        _context.QuestionBanks.Update(bank);

        return await _context.SaveChangesAsync() > 0;
    }

    //==============================
    // DELETE
    //==============================

    public async Task<bool> DeleteAsync(long id)
    {
        var bank = await _context.QuestionBanks.FindAsync(id);

        if (bank == null)
            return false;

        _context.QuestionBanks.Remove(bank);

        return await _context.SaveChangesAsync() > 0;
    }
}