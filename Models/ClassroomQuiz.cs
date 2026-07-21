using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("ClassroomId", "Status", Name = "IX_ClassroomQuizzes_ClassroomId_Status")]
public partial class ClassroomQuiz
{
    [Key]
    public long ClassroomQuizId { get; set; }

    public long ClassroomId { get; set; }

    public long QuizTemplateId { get; set; }

    public long PublishedBy { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Precision(0)]
    public DateTime OpenDate { get; set; }

    [Precision(0)]
    public DateTime DueDate { get; set; }

    public int DurationMinutes { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal PassingScore { get; set; }

    public int MaxAttempts { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(0)]
    public DateTime? PublishedAt { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ClassroomId")]
    [InverseProperty("ClassroomQuizzes")]
    public virtual Classroom Classroom { get; set; } = null!;

    [ForeignKey("PublishedBy")]
    [InverseProperty("ClassroomQuizzes")]
    public virtual User PublishedByNavigation { get; set; } = null!;

    [InverseProperty("ClassroomQuiz")]
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    [ForeignKey("QuizTemplateId")]
    [InverseProperty("ClassroomQuizzes")]
    public virtual QuizTemplate QuizTemplate { get; set; } = null!;
}
