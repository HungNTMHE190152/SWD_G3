using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class LessonService : ILessonService
{
    private readonly EduNexusContext _context;

    public LessonService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<Lesson>> GetByModuleAsync(long moduleId)
    {
        return await _context.Lessons
            .Include(x => x.LessonResources)
            .Where(x => x.ModuleId == moduleId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(long id)
    {
        return await _context.Lessons
            .Include(x => x.Module)
            .Include(x => x.LessonResources)
            .FirstOrDefaultAsync(x => x.LessonId == id);
    }

    public async Task<bool> CreateAsync(Lesson lesson)
    {
        lesson.CreatedAt = DateTime.Now;
        lesson.Status = "DRAFT";

        _context.Lessons.Add(lesson);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Lesson lesson)
    {
        lesson.UpdatedAt = DateTime.UtcNow;

        _context.Lessons.Update(lesson);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var lesson = await _context.Lessons.FindAsync(id);

        if (lesson == null)
            return false;

        _context.Lessons.Remove(lesson);

        return await _context.SaveChangesAsync() > 0;
    }
}