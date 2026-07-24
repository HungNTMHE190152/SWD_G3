using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Assignment.Interfaces
{
    public interface IAssignmentTemplateService
    {
        Task<List<AssignmentTemplateListViewModel>> GetAllAsync(long smeId);

        Task<AssignmentTemplateFormViewModel?> GetByIdAsync(long id, long smeId);

        Task<bool> CreateAsync(AssignmentTemplateFormViewModel model, long smeId);

        Task<bool> UpdateAsync(AssignmentTemplateFormViewModel model, long smeId);

        Task<bool> DeleteAsync(long id, long smeId);

        Task<List<CourseOptionViewModel>> GetCoursesAsync(long smeId);

        Task<List<ModuleOptionViewModel>> GetModulesAsync(long courseId);
    }
}