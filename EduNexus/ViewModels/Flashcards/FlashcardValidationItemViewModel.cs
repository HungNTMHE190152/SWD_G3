namespace EduNexus.ViewModels.Flashcards;

public class FlashcardValidationItemViewModel
{
    public string RuleName { get; set; } = string.Empty;

    public bool IsPassed { get; set; }

    public string Message { get; set; } = string.Empty;
}