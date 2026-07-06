using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("QuizAnswer")]
public partial class QuizAnswer
{
    [Key]
    public long QuizAnswerId { get; set; }

    public long AttemptId { get; set; }

    public long QuestionId { get; set; }

    public long? ChoiceId { get; set; }

    public string? AnswerText { get; set; }

    public bool? IsCorrect { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; }

    [ForeignKey("AttemptId")]
    [InverseProperty("QuizAnswers")]
    public virtual QuizAttempt Attempt { get; set; } = null!;

    [ForeignKey("ChoiceId")]
    [InverseProperty("QuizAnswers")]
    public virtual Choice? Choice { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("QuizAnswers")]
    public virtual Question Question { get; set; } = null!;
}
