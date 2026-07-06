using EduNexus.ViewModels.Dashboard;

namespace EduNexus.Services.Interfaces;

public interface IDashboardService
{
    Task<StudentDashboardViewModel> GetStudentDashboardAsync(long studentId);

    Task<PersonalProgressViewModel> GetPersonalProgressAsync(long studentId);
}