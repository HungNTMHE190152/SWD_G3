using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class ProgressService : IProgressService
{
    private readonly EduNexusContext _context;

    public ProgressService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<bool> CanStudentViewLessonAsync(long studentId, long lessonId)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Module)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

        if (lesson == null || lesson.IsPublished != true)
            return false;

        return await _context.Enrollments.AnyAsync(e =>
            e.StudentId == studentId &&
            e.CourseId == lesson.Module.CourseId &&
            e.Status == "ACTIVE");
    }

    public async Task MarkLessonCompletedAsync(long studentId, long lessonId)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Module)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

        if (lesson == null) return;

        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e =>
                e.StudentId == studentId &&
                e.CourseId == lesson.Module.CourseId);

        if (enrollment == null) return;

        var progress = await _context.Progresses
            .FirstOrDefaultAsync(p =>
                p.EnrollmentId == enrollment.EnrollmentId &&
                p.LessonId == lessonId);

        if (progress == null)
        {
            progress = new Progress
            {
                EnrollmentId = enrollment.EnrollmentId,
                LessonId = lessonId,
                IsCompleted = true,
                CompletionPercentage = 100,
                LastAccess = DateTime.Now,
                CompletedAt = DateTime.Now
            };

            _context.Progresses.Add(progress);
        }
        else
        {
            progress.IsCompleted = true;
            progress.CompletionPercentage = 100;
            progress.LastAccess = DateTime.Now;
            progress.CompletedAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        var courseLessonIds = await _context.Lessons
            .Where(l => l.Module.CourseId == lesson.Module.CourseId && l.IsPublished == true)
            .Select(l => l.LessonId)
            .ToListAsync();

        var totalLessons = courseLessonIds.Count;

        var completedLessons = await _context.Progresses
            .CountAsync(p =>
                p.EnrollmentId == enrollment.EnrollmentId &&
                courseLessonIds.Contains(p.LessonId) &&
                p.IsCompleted == true);

        enrollment.Progress = totalLessons == 0
            ? 0
            : Math.Round((decimal)completedLessons / totalLessons * 100, 2);

        await _context.SaveChangesAsync();
    }
}