using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.QuestionBank;

public class QuestionBankViewModel
{
    public long QuestionBankId { get; set; }

    [Required]
    public long CourseId { get; set; }

    [Required(ErrorMessage = "Bank Name is required")]
    [StringLength(400)]
    public string BankName { get; set; } = "";

    [StringLength(2000)]
    public string? Description { get; set; }

    public bool IsPublic { get; set; }
}