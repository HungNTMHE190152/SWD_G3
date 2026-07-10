using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Flashcards;

public class FlashcardFormViewModel
{
    public long FlashcardId { get; set; }

    public long FlashcardSetId { get; set; }

    [Required(ErrorMessage = "Front Content is required.")]
    [Display(Name = "Front Content")]
    public string FrontContent { get; set; } = string.Empty;

    [Required(ErrorMessage = "Back Content is required.")]
    [Display(Name = "Back Content")]
    public string BackContent { get; set; } = string.Empty;

    [Range(1, 999)]
    public int DisplayOrder { get; set; } = 1;
}