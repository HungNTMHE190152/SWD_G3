using EduNexus.Areas.Admin.ViewModels.Category;
using EduNexus.Services.Common;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryIndexViewModel> GetCategoriesAsync(
            CategoryFilterViewModel filter);

        Task<CategoryFormViewModel?> GetForEditAsync(
            long categoryId);

        Task<ServiceResult<long>> CreateAsync(
            CategoryFormViewModel model);

        Task<ServiceResult> UpdateAsync(
            CategoryFormViewModel model);

        Task<ServiceResult> SetActiveAsync(
            long categoryId,
            bool isActive);
    }
}