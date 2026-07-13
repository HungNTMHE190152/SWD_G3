namespace EduNexus.ViewModels.Quizzes;

public class QuizReviewQuestionViewModel
{
    public long QuestionId { get; set; }

    public string QuestionContent { get; set; } = string.Empty;

    public string? Explanation { get; set; }

    public string? SelectedAnswer { get; set; }

    public string? CorrectAnswer { get; set; }

    public bool IsCorrect { get; set; }

    public decimal? Score { get; set; }
}