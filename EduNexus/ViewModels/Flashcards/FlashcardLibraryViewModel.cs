namespace EduNexus.ViewModels.Flashcards;

public class FlashcardLibraryViewModel
{
    public List<FlashcardLibraryItemViewModel> FlashcardSets { get; set; } = new();
    public int TotalSets { get; set; }
    public int TotalFlashcards { get; set; }
    public int TotalCourses { get; set; }
    public string? Keyword { get; set; }
    public long? CourseId { get; set; }
    public List<CourseFilterItemViewModel> Courses { get; set; } = new();
    public string SortBy { get; set; } = "newest";
}