namespace EduNexus.ViewModels.Flashcards;

public class FlashcardSetItemViewModel
{
    public long FlashcardSetId { get; set; }

    public string SetName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string CourseTitle { get; set; } = string.Empty;

    public int TotalCards { get; set; }

    public bool IsAIGenerated { get; set; }

    public DateTime? CreatedAt { get; set; }
}
