using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("QuizQuestion")]
[Index("QuizId", "QuestionId", Name = "UQ_QuizQuestion", IsUnique = true)]
public partial class QuizQuestion
{
    [Key]
    public long QuizQuestionId { get; set; }

    public long QuizId { get; set; }

    public long QuestionId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; }

    public int? DisplayOrder { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("QuizQuestions")]
    public virtual Question Question { get; set; } = null!;

    [ForeignKey("QuizId")]
    [InverseProperty("QuizQuestions")]
    public virtual Quiz Quiz { get; set; } = null!;
}
