namespace EduNexus.Services.Interfaces;

public interface IProgressService
{
    Task<bool> CanStudentViewLessonAsync(long studentId, long lessonId);
    Task MarkLessonCompletedAsync(long studentId, long lessonId);
}