namespace EduNexus.ViewModels.Dashboard;

public class RecentAssignmentResultViewModel
{
    public long SubmissionId { get; set; }

    public string AssignmentTitle { get; set; } = string.Empty;

    public decimal? TotalScore { get; set; }

    public string GradingStatus { get; set; } = string.Empty;

    public DateTime? SubmittedAt { get; set; }
}