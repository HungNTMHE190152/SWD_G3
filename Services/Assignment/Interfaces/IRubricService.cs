using EduNexus.ViewModels.Assignment;

namespace EduNexus.Services.Assignment.Interfaces
{
    public interface IRubricService
    {
        Task<List<RubricViewModel>> GetAllAsync(long smeId);

        Task<RubricViewModel?> GetByIdAsync(long rubricId);

        Task<RubricViewModel> GetCreateModelAsync(long assignmentId);

        Task<bool> CreateAsync(RubricViewModel model);

        Task<bool> UpdateAsync(RubricViewModel model);

        Task<bool> DeleteAsync(long rubricId);
    }
}