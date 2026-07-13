using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Questions;

public class QuestionFormViewModel
{
    public long QuestionId { get; set; }

    [Required]
    public long BankId { get; set; }

    public string BankName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Question content is required.")]
    public string QuestionContent { get; set; } = string.Empty;

    [Required]
    public string QuestionType { get; set; } = "MULTIPLE_CHOICE";

    public string? Difficulty { get; set; } = "EASY";

    public string? Explanation { get; set; }

    public List<ChoiceFormViewModel> Choices { get; set; } = new()
    {
        new ChoiceFormViewModel { DisplayOrder = 1 },
        new ChoiceFormViewModel { DisplayOrder = 2 },
        new ChoiceFormViewModel { DisplayOrder = 3 },
        new ChoiceFormViewModel { DisplayOrder = 4 }
    };
}