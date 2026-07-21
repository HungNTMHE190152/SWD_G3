using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("QuizTemplateId", "DisplayOrder", Name = "UQ_QuizTemplateQuestions_Template_Order", IsUnique = true)]
[Index("QuizTemplateId", "QuestionId", Name = "UQ_QuizTemplateQuestions_Template_Question", IsUnique = true)]
public partial class QuizTemplateQuestion
{
    [Key]
    public long QuizTemplateQuestionId { get; set; }

    public long QuizTemplateId { get; set; }

    public long QuestionId { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal Score { get; set; }

    public int DisplayOrder { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("QuizTemplateQuestions")]
    public virtual Question Question { get; set; } = null!;

    [ForeignKey("QuizTemplateId")]
    [InverseProperty("QuizTemplateQuestions")]
    public virtual QuizTemplate QuizTemplate { get; set; } = null!;
}
