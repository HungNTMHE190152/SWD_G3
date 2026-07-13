namespace EduNexus.ViewModels.Quizzes;

public class QuizHistoryViewModel
{
    public List<QuizHistoryItemViewModel> Attempts { get; set; } = new();
}

public class QuizHistoryItemViewModel
{
    public long AttemptId { get; set; }
    public long QuizId { get; set; }

    public string QuizTitle { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;

    public int AttemptNumber { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime? SubmitTime { get; set; }

    public decimal? TotalScore { get; set; }
    public decimal? PassingScore { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool IsPassed { get; set; }
}