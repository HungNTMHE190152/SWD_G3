using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Flashcards;

public class FlashcardSetFormViewModel
{
    public long FlashcardSetId { get; set; }

    [Required(ErrorMessage = "Course is required.")]
    public long CourseId { get; set; }

    [Required(ErrorMessage = "Set Name is required.")]
    [StringLength(200)]
    public string SetName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsAIGenerated { get; set; }

    // Dùng để đổ dropdown Course
    public List<CourseOptionViewModel> Courses { get; set; } = new();
}