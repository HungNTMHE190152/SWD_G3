using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface ILessonService
{
    Task<List<Lesson>> GetByModuleAsync(long moduleId);

    Task<Lesson?> GetByIdAsync(long id);

    Task<bool> CreateAsync(Lesson lesson);

    Task<bool> UpdateAsync(Lesson lesson);

    Task<bool> DeleteAsync(long id);
}