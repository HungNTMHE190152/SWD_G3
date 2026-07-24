using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.SME.Implementations;

public class CourseService : ICourseService
{
    private readonly EduNexusContext _context;

    public CourseService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(x => x.Category)
            .Include(x => x.Modules)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(long id)
    {
        return await _context.Courses
            .Include(x => x.Category)
            .Include(x => x.Modules)
            .Include(x => x.QuestionBanks)
            .Include(x => x.QuizTemplates)
            .FirstOrDefaultAsync(x => x.CourseId == id);
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.Categories
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(Course course)
    {
        course.Status = "DRAFT";
        course.CreatedAt = DateTime.UtcNow;

        _context.Courses.Add(course);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        course.UpdatedAt = DateTime.UtcNow;

        _context.Courses.Update(course);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return false;

        _context.Courses.Remove(course);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> SubmitForReviewAsync(long id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return false;

        course.Status = "PENDING_REVIEW";
        course.SubmittedAt = DateTime.UtcNow;

        return await _context.SaveChangesAsync() > 0;
    }
}