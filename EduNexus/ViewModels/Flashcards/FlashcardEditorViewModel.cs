namespace EduNexus.ViewModels.Flashcards;

public class FlashcardEditorViewModel
{
    public long FlashcardSetId { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<FlashcardItemViewModel> Flashcards { get; set; } = new();
}