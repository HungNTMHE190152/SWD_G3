using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Questions;

public class QuestionBankFormViewModel
{
    public long BankId { get; set; }

    [Required(ErrorMessage = "Please select a course.")]
    public long CourseId { get; set; }

    [Required(ErrorMessage = "Bank name is required.")]
    [StringLength(200)]
    public string BankName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsPublic { get; set; }

    public List<SelectListItem> Courses { get; set; } = new();
}