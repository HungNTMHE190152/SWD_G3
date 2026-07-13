namespace EduNexus.ViewModels.Quizzes;

public class NewQuizViewModel
{
    public List<AvailableQuizItemViewModel> Quizzes { get; set; } = new();
}

public class AvailableQuizItemViewModel
{
    public long QuizId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string CourseTitle { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;

    public int Duration { get; set; }
    public decimal? PassingScore { get; set; }
    public int QuestionCount { get; set; }

    public int AttemptCount { get; set; }
    public decimal? LastScore { get; set; }
}