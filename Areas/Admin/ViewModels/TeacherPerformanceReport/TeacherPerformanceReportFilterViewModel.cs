namespace EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport
{
    public class TeacherPerformanceReportFilterViewModel
    {
        public string? SearchText { get; set; }

        public string DataStatus { get; set; } = "ALL";

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}