using EduNexus.Models;

namespace EduNexus.Services.Interfaces;

public interface ILessonService
{
    Task<List<Lesson>> GetLessonsByModuleAsync(long moduleId);
    Task<List<Lesson>> GetPublishedLessonsForStudentAsync(long studentId);
    Task<Lesson?> GetLessonDetailAsync(long lessonId);
    Task CreateLessonAsync(Lesson lesson);
    Task UpdateLessonAsync(Lesson lesson);
    Task DeleteLessonAsync(long lessonId);
    Task PublishLessonAsync(long lessonId);
    
}