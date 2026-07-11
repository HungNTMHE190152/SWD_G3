using EduNexus.ViewModels.Flashcards;

namespace EduNexus.Services.Interfaces;

public interface IFlashcardService
{
    Task<FlashcardSetListViewModel> GetMyFlashcardSetsAsync(long userId);

    Task<FlashcardEditorViewModel?> GetFlashcardEditorAsync(long flashcardSetId);

    Task CreateFlashcardAsync(FlashcardFormViewModel model);

    Task UpdateFlashcardAsync(FlashcardFormViewModel model);

    Task DeleteFlashcardAsync(long flashcardId);

    // Flashcard Set
    Task<FlashcardSetFormViewModel> GetCreateSetViewModelAsync();

    Task CreateFlashcardSetAsync(long userId, FlashcardSetFormViewModel model);

    Task<FlashcardSetFormViewModel?> GetEditSetViewModelAsync(long flashcardSetId);

    Task UpdateFlashcardSetAsync(long userId, FlashcardSetFormViewModel model);

    Task DeleteFlashcardSetAsync(long flashcardSetId);
    Task<FlashcardStagingViewModel?> GetFlashcardStagingAsync(long flashcardSetId);
    Task<FlashcardPreviewViewModel?> GetFlashcardPreviewAsync(long flashcardSetId);
    Task<FlashcardLibraryViewModel> GetFlashcardLibraryAsync(string? keyword,long? courseId,string? sortBy);
    Task<FlashcardPracticeViewModel?> GetFlashcardPracticeAsync(long flashcardSetId);
    Task SavePracticeAsync(long studentId,FlashcardPracticeSubmitViewModel model);
}