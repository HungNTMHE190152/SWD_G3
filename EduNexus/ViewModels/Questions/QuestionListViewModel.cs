namespace EduNexus.ViewModels.Questions;

public class QuestionListViewModel
{
    public long BankId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;

    public List<QuestionListItemViewModel> Questions { get; set; } = new();
}