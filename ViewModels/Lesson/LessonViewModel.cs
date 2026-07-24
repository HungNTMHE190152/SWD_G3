using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Lesson;

public class LessonViewModel
{
    public long LessonId { get; set; }

    public long ModuleId { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string LessonType { get; set; } = "TEXT";

    public string? Content { get; set; }

    [Range(1, 500)]
    public int? EstimatedMinutes { get; set; }

    [Range(1, 1000)]
    public int DisplayOrder { get; set; } = 1;
}