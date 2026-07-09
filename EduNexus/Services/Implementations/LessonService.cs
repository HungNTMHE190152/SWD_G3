using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class LessonService : ILessonService
{
    private readonly EduNexusContext _context;

    public LessonService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<List<Lesson>> GetLessonsByModuleAsync(long moduleId)
    {
        return await _context.Lessons
            .Where(l => l.ModuleId == moduleId)
            .OrderBy(l => l.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Lesson?> GetLessonDetailAsync(long lessonId)
    {
        return await _context.Lessons
            .Include(l => l.Module)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);
    }

    public async Task CreateLessonAsync(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task PublishLessonAsync(long lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);

        if (lesson == null) return;

        lesson.IsPublished = true;
        await _context.SaveChangesAsync();
    }
    public async Task UpdateLessonAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLessonAsync(long lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);

        if (lesson == null) return;

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Lesson>> GetPublishedLessonsForStudentAsync(long studentId)
    {
        var enrolledCourseIds = await _context.Enrollments
            .Where(e => e.StudentId == studentId && e.CourseId != null && e.Status == "ACTIVE")
            .Select(e => e.CourseId!.Value)
            .ToListAsync();

        return await _context.Lessons
            .Include(l => l.Module)
            .Where(l =>
                l.IsPublished == true &&
                enrolledCourseIds.Contains(l.Module.CourseId))
            .OrderBy(l => l.Module.CourseId)
            .ThenBy(l => l.Module.DisplayOrder)
            .ThenBy(l => l.DisplayOrder)
            .ToListAsync();
    }
}