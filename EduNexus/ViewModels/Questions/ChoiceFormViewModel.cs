namespace EduNexus.ViewModels.Questions;

public class ChoiceFormViewModel
{
    public long ChoiceId { get; set; }

    public string ChoiceContent { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    public int DisplayOrder { get; set; }
}