using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;

namespace EduNexus.Services.Implementations;

public class AiLessonService : IAiLessonService
{
    private readonly EduNexusContext _context;

    public AiLessonService(EduNexusContext context)
    {
        _context = context;
    }

    public Task<Lesson> GenerateFakeLessonAsync(long moduleId, string prompt)
    {
        var lesson = new Lesson
        {
            ModuleId = moduleId,
            LessonName = "AI Generated Lesson",
            LessonType = "MARKDOWN",
            Content = $"This is fake AI generated lesson content based on prompt: {prompt}",
            Duration = 15,
            DisplayOrder = 99,
            IsPublished = false,
            CreatedAt = DateTime.Now
        };

        return Task.FromResult(lesson);
    }
}