using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Quiz")]
[Index("AssignmentId", Name = "UQ_Quiz_Assignment", IsUnique = true)]
public partial class Quiz
{
    [Key]
    public long QuizId { get; set; }

    public long AssignmentId { get; set; }

    [StringLength(200)]
    public string QuizTitle { get; set; } = null!;

    public string? Description { get; set; }

    public int Duration { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? PassingScore { get; set; }

    public bool? ShuffleQuestion { get; set; }

    public bool? ShuffleAnswer { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AssignmentId")]
    [InverseProperty("Quiz")]
    public virtual Assignment Assignment { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("Quizzes")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Quiz")]
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    [InverseProperty("Quiz")]
    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
