using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class LessonResourceService : ILessonResourceService
{
    private readonly EduNexusContext _context;

    public LessonResourceService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<LessonResource>> GetByLessonAsync(long lessonId)
    {
        return await _context.LessonResources
            .Where(x => x.LessonId == lessonId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<LessonResource?> GetByIdAsync(long id)
    {
        return await _context.LessonResources
            .Include(x => x.Lesson)
            .FirstOrDefaultAsync(x => x.LessonResourceId == id);
    }

    public async Task<bool> CreateAsync(LessonResource resource)
    {
        resource.CreatedAt = DateTime.UtcNow;

        _context.LessonResources.Add(resource);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(LessonResource resource)
    {
        _context.LessonResources.Update(resource);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var resource = await _context.LessonResources.FindAsync(id);

        if (resource == null)
            return false;

        _context.LessonResources.Remove(resource);

        return await _context.SaveChangesAsync() > 0;
    }
}