using EduNexus.Areas.Admin.ViewModels.RankingConfiguration;
using EduNexus.Services.Common;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface IRankingConfigurationService
    {
        Task<RankingConfigurationIndexViewModel>
            GetIndexAsync();

        Task<ServiceResult> SaveConfigurationAsync(
            RankingConfigurationFormViewModel model,
            long updatedBy);
    }
}