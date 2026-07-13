namespace EduNexus.ViewModels.Questions;

public class QuestionListItemViewModel
{
    public long QuestionId { get; set; }
    public long BankId { get; set; }
    public string QuestionContent { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public string? Difficulty { get; set; }
    public string? Explanation { get; set; }
    public int ChoiceCount { get; set; }
    public int CorrectChoiceCount { get; set; }
    public bool IsAIGenerated { get; set; }
    public DateTime? CreatedAt { get; set; }
}