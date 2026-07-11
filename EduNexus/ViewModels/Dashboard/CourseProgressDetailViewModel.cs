namespace EduNexus.ViewModels.Dashboard;

public class CourseProgressDetailViewModel
{
    public long EnrollmentId { get; set; }

    public long CourseId { get; set; }

    public string CourseTitle { get; set; } = string.Empty;

    public int CompletedLessons { get; set; }

    public int TotalLessons { get; set; }

    public decimal ProgressPercentage { get; set; }
}