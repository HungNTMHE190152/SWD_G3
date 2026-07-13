namespace EduNexus.ViewModels.Questions;

public class QuestionBankListItemViewModel
{
    public long BankId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public int QuestionCount { get; set; }
    public DateTime? CreatedAt { get; set; }
}