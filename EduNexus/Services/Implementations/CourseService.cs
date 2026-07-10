using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly EduNexusContext _context;

    public CourseService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetCoursesBySmeAsync(long smeId)
    {
        return await _context.Courses
            .Where(c => c.CreatedBy == smeId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Course?> GetCourseStructureAsync(long courseId)
    {
        return await _context.Courses
            .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
            .FirstOrDefaultAsync(c => c.CourseId == courseId);
    }
}