using EduNexus.Models;

namespace EduNexus.Services.Interfaces;

public interface IAiLessonService
{
    Task<Lesson> GenerateFakeLessonAsync(long moduleId, string prompt);
}