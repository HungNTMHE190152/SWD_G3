using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Question;

public class QuestionViewModel
{
    public long QuestionId { get; set; }

    [Required]
    public long QuestionBankId { get; set; }

    [Required(ErrorMessage = "Question Content is required")]
    public string QuestionContent { get; set; } = "";

    [Required]
    public string QuestionType { get; set; } = "MCQ";

    [Required]
    public string Difficulty { get; set; } = "EASY";

    public string? Explanation { get; set; }
}