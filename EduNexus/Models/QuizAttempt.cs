using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("QuizAttempt")]
[Index("StudentId", Name = "IX_QuizAttempt_StudentId")]
[Index("QuizId", "StudentId", "AttemptNumber", Name = "UQ_QuizAttempt", IsUnique = true)]
public partial class QuizAttempt
{
    [Key]
    public long AttemptId { get; set; }

    public long QuizId { get; set; }

    public long StudentId { get; set; }

    public int? AttemptNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SubmitTime { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? TotalScore { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    [ForeignKey("QuizId")]
    [InverseProperty("QuizAttempts")]
    public virtual Quiz Quiz { get; set; } = null!;

    [InverseProperty("Attempt")]
    public virtual ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();

    [ForeignKey("StudentId")]
    [InverseProperty("QuizAttempts")]
    public virtual User Student { get; set; } = null!;
}
