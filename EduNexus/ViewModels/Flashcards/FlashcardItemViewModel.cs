namespace EduNexus.ViewModels.Flashcards;

public class FlashcardItemViewModel
{
    public long FlashcardId { get; set; }

    public string FrontContent { get; set; } = string.Empty;

    public string BackContent { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}