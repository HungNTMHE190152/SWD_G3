namespace EduNexus.ViewModels.Flashcards;

public class FlashcardPracticeSubmitViewModel
{
    public List<FlashcardPracticeAnswerViewModel> Answers { get; set; } = new();
}

public class FlashcardPracticeAnswerViewModel
{
    public long FlashcardId { get; set; }

    public bool IsCorrect { get; set; }
}