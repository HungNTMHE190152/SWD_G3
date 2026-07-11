

using EduNexus.ViewModels.Rubric;

namespace EduNexus.Services
{
    public interface IRubricService
    {
        Task<List<RubricListViewModel>> GetRubricsAsync(long assignmentId);
        Task<RubricDetailViewModel?> GetByIdAsync(long rubricId);
        Task<CreateRubricViewModel> GetCreateModelAsync(long assignmentId);
        Task CreateAsync(CreateRubricViewModel model);
        Task<EditRubricViewModel?> GetEditModelAsync(long rubricId);
        Task UpdateAsync(EditRubricViewModel model);
        Task DeleteAsync(long rubricId);
    }
}