using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface ILessonResourceService
{
    Task<List<LessonResource>> GetByLessonAsync(long lessonId);

    Task<LessonResource?> GetByIdAsync(long id);

    Task<bool> CreateAsync(LessonResource resource);

    Task<bool> UpdateAsync(LessonResource resource);

    Task<bool> DeleteAsync(long id);
}