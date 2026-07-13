namespace EduNexus.ViewModels.Quizzes;

public class QuizReviewViewModel
{
    public long AttemptId { get; set; }

    public string QuizTitle { get; set; } = string.Empty;

    public decimal? TotalScore { get; set; }

    public List<QuizReviewQuestionViewModel> Questions { get; set; } = new();
}