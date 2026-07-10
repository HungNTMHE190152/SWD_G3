namespace EduNexus.ViewModels.Flashcards;

public class FlashcardPracticeViewModel
{
    public long FlashcardSetId { get; set; }

    public string SetName { get; set; } = string.Empty;

    public int TotalCards { get; set; }

    public List<FlashcardPracticeItemViewModel> Flashcards { get; set; } = new();
}