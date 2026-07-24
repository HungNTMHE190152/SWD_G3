using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class ModuleService : IModuleService
{
    private readonly EduNexusContext _context;

    public ModuleService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<Module>> GetAllAsync()
    {
        return await _context.Modules
            .Include(x => x.Course)
            .Include(x => x.Lessons)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<List<Module>> GetByCourseAsync(long courseId)
    {
        return await _context.Modules
            .Where(x => x.CourseId == courseId)
            .Include(x => x.Lessons)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Module?> GetByIdAsync(long id)
    {
        return await _context.Modules
            .Include(x => x.Course)
            .Include(x => x.Lessons)
            .FirstOrDefaultAsync(x => x.ModuleId == id);
    }

    public async Task<List<Course>> GetCoursesAsync()
    {
        return await _context.Courses
            .OrderBy(x => x.Title)
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(Module module)
    {
        module.CreatedAt = DateTime.UtcNow;

        _context.Modules.Add(module);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Module module)
    {
        module.UpdatedAt = DateTime.UtcNow;

        _context.Modules.Update(module);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var module = await _context.Modules.FindAsync(id);

        if (module == null)
            return false;

        _context.Modules.Remove(module);

        return await _context.SaveChangesAsync() > 0;
    }
}