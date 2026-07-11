namespace EduNexus.ViewModels.Dashboard;

public class StudentCourseItemViewModel
{
    public long EnrollmentId { get; set; }

    public long CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public decimal ProgressPercentage { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? EnrollDate { get; set; }
}