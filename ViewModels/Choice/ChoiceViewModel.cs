using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Choice;

public class ChoiceViewModel
{
    public long ChoiceId { get; set; }

    [Required]
    public long QuestionId { get; set; }

    [Required(ErrorMessage = "Choice Content is required")]
    public string ChoiceContent { get; set; } = "";

    public bool IsCorrect { get; set; }

    [Required]
    public int DisplayOrder { get; set; }
}