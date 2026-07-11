namespace EduNexus.ViewModels.Dashboard;

public class StudentPackageItemViewModel
{
    public long EnrollmentId { get; set; }

    public long CourseGroupId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal ProgressPercentage { get; set; }

    public DateTime? EnrollDate { get; set; }
}