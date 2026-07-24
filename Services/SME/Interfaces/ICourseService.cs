using EduNexus.Models;

namespace EduNexus.Services.SME.Interfaces;

public interface ICourseService
{
    Task<List<EduNexus.Models.Course>> GetAllAsync();

    Task<EduNexus.Models.Course?> GetByIdAsync(long id);

    Task<List<Category>> GetCategoriesAsync();

    Task<bool> CreateAsync(EduNexus.Models.Course course);

    Task<bool> UpdateAsync(EduNexus.Models.Course course);

    Task<bool> DeleteAsync(long id);

    Task<bool> SubmitForReviewAsync(long id);
}