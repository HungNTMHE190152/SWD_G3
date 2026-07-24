using EduNexus.Areas.Admin.ViewModels.TeacherPerformanceConfiguration;
using EduNexus.Services.Common;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface ITeacherPerformanceConfigurationService
    {
        Task<TeacherPerformanceConfigurationIndexViewModel>
            GetIndexAsync();

        Task<ServiceResult> SaveConfigurationAsync(
            TeacherPerformanceConfigurationFormViewModel model,
            long updatedBy);
    }
}