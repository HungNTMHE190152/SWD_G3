namespace EduNexus.ViewModels.Quizzes;

public class QuizResultViewModel
{
    public long AttemptId { get; set; }
    public long QuizId { get; set; }

    public string QuizTitle { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
    public DateTime? SubmitTime { get; set; }

    public decimal? TotalScore { get; set; }
    public decimal? PassingScore { get; set; }

    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public int WrongAnswers { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool IsPassed { get; set; }
}