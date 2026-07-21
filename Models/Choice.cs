using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("QuestionId", "DisplayOrder", Name = "UQ_Choices_Question_DisplayOrder", IsUnique = true)]
public partial class Choice
{
    [Key]
    public long ChoiceId { get; set; }

    public long QuestionId { get; set; }

    [StringLength(1000)]
    public string ChoiceContent { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int DisplayOrder { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("Choices")]
    public virtual Question Question { get; set; } = null!;

    [InverseProperty("Choice")]
    public virtual ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();
}
