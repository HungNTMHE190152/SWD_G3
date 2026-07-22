using EduNexus.Areas.Admin.ViewModels.Profile;
using EduNexus.Services.Common;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface IAdminProfileService
    {
        Task<AdminProfileViewModel?> GetProfileAsync(
            long userId);

        Task<EditAdminProfileViewModel?> GetEditAsync(
            long userId);

        Task<ServiceResult> UpdateProfileAsync(
            long userId,
            EditAdminProfileViewModel model);
    }
}