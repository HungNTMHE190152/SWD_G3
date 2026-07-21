using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("CourseId", "IsActive", Name = "IX_QuizTemplates_CourseId_IsActive")]
public partial class QuizTemplate
{
    [Key]
    public long QuizTemplateId { get; set; }

    public long CourseId { get; set; }

    public long? ModuleId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public int DefaultDurationMinutes { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal DefaultPassingScore { get; set; }

    public int DefaultMaxAttempts { get; set; }

    public bool ShuffleQuestions { get; set; }

    public bool ShuffleAnswers { get; set; }

    public bool IsActive { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("QuizTemplate")]
    public virtual ICollection<ClassroomQuiz> ClassroomQuizzes { get; set; } = new List<ClassroomQuiz>();

    [ForeignKey("CourseId")]
    [InverseProperty("QuizTemplates")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("QuizTemplates")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [ForeignKey("ModuleId")]
    [InverseProperty("QuizTemplates")]
    public virtual Module? Module { get; set; }

    [InverseProperty("QuizTemplate")]
    public virtual ICollection<QuizTemplateQuestion> QuizTemplateQuestions { get; set; } = new List<QuizTemplateQuestion>();
}
