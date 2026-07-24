using EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface ITeacherPerformanceReportService
    {
        Task<TeacherPerformanceReportIndexViewModel>
            GetReportAsync(
                TeacherPerformanceReportFilterViewModel filter);

        Task<TeacherPerformanceDetailsViewModel?>
            GetDetailsAsync(long teacherId);
    }
}