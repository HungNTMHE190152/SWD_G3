using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Quizzes;

public class QuizFormViewModel
{
    [Required(ErrorMessage = "Please select a module.")]
    public long ModuleId { get; set; }

    [Required(ErrorMessage = "Quiz title is required.")]
    [StringLength(200)]
    public string QuizTitle { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(1, 300, ErrorMessage = "Duration must be from 1 to 300 minutes.")]
    public int Duration { get; set; } = 15;

    [Range(0, 10, ErrorMessage = "Passing score must be from 0 to 10.")]
    public decimal? PassingScore { get; set; } = 5;

    public bool ShuffleQuestion { get; set; }

    public bool ShuffleAnswer { get; set; }

    public List<SelectListItem> Modules { get; set; } = new();
}