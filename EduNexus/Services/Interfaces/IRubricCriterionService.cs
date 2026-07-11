using EduNexus.ViewModels.Rubric;
using EduNexus.ViewModels.RubricCriterion;


namespace EduNexus.Services.Interfaces
{
    public interface IRubricCriterionService
    {
        Task<RubricManagementViewModel> GetCriteriaAsync(long rubricId);
        Task AddCriterionAsync(CreateCriterionViewModel model);
        Task UpdateCriteriaAsync(List<RubricCriterionItemViewModel> criteria);
        Task RemoveCriterionAsync(long criterionId);
        Task<EditCriterionViewModel?> GetForEditAsync(long id);
        Task UpdateCriterionAsync(EditCriterionViewModel model);
    }
}
