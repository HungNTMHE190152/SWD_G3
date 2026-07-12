namespace EduNexus.ViewModels.Flashcards;

public class FlashcardStagingViewModel
{
    public long FlashcardSetId { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TotalCards { get; set; }
    public List<FlashcardItemViewModel> Flashcards { get; set; } = new();
    public List<FlashcardValidationItemViewModel> ValidationResults { get; set; } = new();
    public int PassedRules { get; set; }
    public int TotalRules { get; set; }
    public bool IsReady => PassedRules == TotalRules;
    public int AverageQuestionLength { get; set; }
    public int AverageAnswerLength { get; set; }
    public int MaxDisplayOrder { get; set; }
    public int EmptyQuestionCount { get; set; }
    public int EmptyAnswerCount { get; set; }
}