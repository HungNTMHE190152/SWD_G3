namespace EduNexus.ViewModels.Flashcards;

public class FlashcardPreviewViewModel
{
    public long FlashcardSetId { get; set; }

    public string SetName { get; set; } = string.Empty;

    public List<FlashcardItemViewModel> Flashcards { get; set; } = new();
}