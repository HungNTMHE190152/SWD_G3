namespace EduNexus.ViewModels.Flashcards;

public class FlashcardLibraryItemViewModel
{
    public long FlashcardSetId { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TotalCards { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastReviewed { get; set; }
    public int TotalReviews { get; set; }
    public int TotalCorrect { get; set; }
    public double Accuracy { get; set; }
    public bool HasPracticed { get; set; }
}