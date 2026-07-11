using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations;

public class LessonTranscriptService : ILessonTranscriptService
{
    private readonly EduNexusContext _context;

    public LessonTranscriptService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<LessonTranscript?> GetByLessonIdAsync(long lessonId)
    {
        return await _context.LessonTranscripts
            .FirstOrDefaultAsync(t => t.LessonId == lessonId);
    }

    public async Task GenerateFakeAsync(long lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        if (lesson == null) return;

        var transcript = await _context.LessonTranscripts
            .FirstOrDefaultAsync(t => t.LessonId == lessonId);

        if (transcript == null)
        {
            transcript = new LessonTranscript
            {
                LessonId = lessonId,
                CreatedAt = DateTime.Now
            };
            _context.LessonTranscripts.Add(transcript);
        }

        transcript.RawTranscript = $"Fake transcript for lesson: {lesson.LessonName}. This lesson explains {lesson.Content}";
        transcript.FormattedSummary = $"Summary: {lesson.LessonName} covers basic concepts and key learning points.";
        transcript.Status = "DONE";

        await _context.SaveChangesAsync();
    }
}