using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("QuizAttemptId", "QuestionId", Name = "UQ_QuizAnswers_Attempt_Question", IsUnique = true)]
public partial class QuizAnswer
{
    [Key]
    public long QuizAnswerId { get; set; }

    public long QuizAttemptId { get; set; }

    public long QuestionId { get; set; }

    public long? ChoiceId { get; set; }

    public bool IsCorrect { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal Score { get; set; }

    [Precision(0)]
    public DateTime? AnsweredAt { get; set; }

    [ForeignKey("ChoiceId")]
    [InverseProperty("QuizAnswers")]
    public virtual Choice? Choice { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("QuizAnswers")]
    public virtual Question Question { get; set; } = null!;

    [ForeignKey("QuizAttemptId")]
    [InverseProperty("QuizAnswers")]
    public virtual QuizAttempt QuizAttempt { get; set; } = null!;
}
