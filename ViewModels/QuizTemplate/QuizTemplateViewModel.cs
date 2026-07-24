using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.QuizTemplate;

public class QuizTemplateViewModel
{
    public long QuizTemplateId { get; set; }

    [Required]
    public long CourseId { get; set; }

    public long? ModuleId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = "";

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(1, 300)]
    public int DefaultDurationMinutes { get; set; } = 30;

    [Range(0, 100)]
    public decimal DefaultPassingScore { get; set; } = 5;

    [Range(1, 20)]
    public int DefaultMaxAttempts { get; set; } = 1;

    public bool ShuffleQuestions { get; set; }

    public bool ShuffleAnswers { get; set; }

    public bool IsActive { get; set; } = true;
}