namespace EduNexus.ViewModels.Dashboard;

public class QuizProgressItemViewModel
{
    public long AttemptId { get; set; }

    public string QuizTitle { get; set; } = string.Empty;

    public decimal? TotalScore { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
}