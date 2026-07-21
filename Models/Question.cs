using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("QuestionBankId", Name = "IX_Questions_QuestionBankId")]
public partial class Question
{
    [Key]
    public long QuestionId { get; set; }

    public long QuestionBankId { get; set; }

    public long CreatedBy { get; set; }

    public string QuestionContent { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string QuestionType { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Difficulty { get; set; } = null!;

    public string? Explanation { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<Choice> Choices { get; set; } = new List<Choice>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("Questions")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [ForeignKey("QuestionBankId")]
    [InverseProperty("Questions")]
    public virtual QuestionBank QuestionBank { get; set; } = null!;

    [InverseProperty("Question")]
    public virtual ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();

    [InverseProperty("Question")]
    public virtual ICollection<QuizTemplateQuestion> QuizTemplateQuestions { get; set; } = new List<QuizTemplateQuestion>();
}
