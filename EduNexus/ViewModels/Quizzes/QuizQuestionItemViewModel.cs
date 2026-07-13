namespace EduNexus.ViewModels.Quizzes;

public class QuizQuestionItemViewModel
{
    public long QuestionId { get; set; }

    public string QuestionContent { get; set; } = string.Empty;

    public string QuestionType { get; set; } = string.Empty;

    public string? Difficulty { get; set; }

    public int ChoiceCount { get; set; }

    public bool IsSelected { get; set; }

    public decimal Score { get; set; } = 1;

    public int DisplayOrder { get; set; } = 1;
}