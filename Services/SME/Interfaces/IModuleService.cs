using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface IModuleService
{
    Task<List<Module>> GetAllAsync();

    Task<List<Module>> GetByCourseAsync(long courseId);

    Task<Module?> GetByIdAsync(long id);

    Task<List<Course>> GetCoursesAsync();

    Task<bool> CreateAsync(Module module);

    Task<bool> UpdateAsync(Module module);

    Task<bool> DeleteAsync(long id);
}