using EduNexus.Areas.Admin.ViewModels.UserManagement;
using EduNexus.Services.Common;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface IUserManagementService
    {
        Task<UserManagementIndexViewModel> GetUsersAsync(
            UserFilterViewModel filter);

        Task<UserDetailsViewModel?> GetDetailsAsync(
            long userId);

        Task<List<RoleOptionViewModel>> GetRolesAsync();

        Task<ServiceResult<long>> CreateUserAsync(
            CreateUserViewModel model);

        Task<ServiceResult> SetAccountActiveAsync(
            long userId,
            bool isActive,
            long currentAdminUserId);

        Task<ServiceResult> ChangeRoleAsync(
            long userId,
            long newRoleId,
            long currentAdminUserId);
    }
}