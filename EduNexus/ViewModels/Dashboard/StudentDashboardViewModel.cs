namespace EduNexus.ViewModels.Dashboard;

public class StudentDashboardViewModel
{
    public string StudentName { get; set; } = string.Empty;

    public int TotalCourses { get; set; }

    public int TotalPackages { get; set; }

    public int CompletedLessons { get; set; }

    public int TotalLessons { get; set; }

    public decimal OverallProgress { get; set; }

    public List<StudentCourseItemViewModel> Courses { get; set; } = new();

    public List<StudentPackageItemViewModel> Packages { get; set; } = new();

    public List<RecentQuizAttemptViewModel> RecentQuizAttempts { get; set; } = new();

    public List<RecentAssignmentResultViewModel> RecentAssignmentResults { get; set; } = new();
}