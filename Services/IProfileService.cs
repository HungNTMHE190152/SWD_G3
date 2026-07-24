using EduNexus.Services.Common;
using EduNexus.ViewModels.Profile;

namespace EduNexus.Services.Interfaces
{
    public interface IProfileService
    {
        Task<UserProfileViewModel?> GetProfileAsync(
            long userId);

        Task<EditProfileViewModel?> GetEditAsync(
            long userId);

        Task<ServiceResult> UpdateProfileAsync(
            long userId,
            EditProfileViewModel model);
    }
}