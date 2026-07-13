namespace EduNexus.ViewModels.Quizzes;

public class QuizListItemViewModel
{
    public long QuizId { get; set; }
    public long AssignmentId { get; set; }

    public string QuizTitle { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string CourseTitle { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;

    public int Duration { get; set; }
    public decimal? PassingScore { get; set; }

    public int QuestionCount { get; set; }

    public bool ShuffleQuestion { get; set; }
    public bool ShuffleAnswer { get; set; }

    public DateTime? CreatedAt { get; set; }
}