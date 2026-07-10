using EduNexus.Models;

namespace EduNexus.Services.Interfaces;

public interface ILessonTranscriptService
{
    Task<LessonTranscript?> GetByLessonIdAsync(long lessonId);
    Task GenerateFakeAsync(long lessonId);
}