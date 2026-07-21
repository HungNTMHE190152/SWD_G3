using EduNexus.Areas.Admin.ViewModels.Dashboard;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardViewModel> GetDashboardAsync();
    }
}