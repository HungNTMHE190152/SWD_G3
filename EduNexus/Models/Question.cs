using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Question")]
[Index("BankId", Name = "IX_Question_BankId")]
public partial class Question
{
    [Key]
    public long QuestionId { get; set; }

    public long BankId { get; set; }

    public long CreatedBy { get; set; }

    public string QuestionContent { get; set; } = null!;

    [StringLength(30)]
    public string QuestionType { get; set; } = null!;

    [StringLength(20)]
    public string? Difficulty { get; set; }

    public string? Explanation { get; set; }

    [Column("IsAIGenerated")]
    public bool? IsAigenerated { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<AssignmentQuestion> AssignmentQuestions { get; set; } = new List<AssignmentQuestion>();

    [ForeignKey("BankId")]
    [InverseProperty("Questions")]
    public virtual QuestionBank Bank { get; set; } = null!;

    [InverseProperty("Question")]
    public virtual ICollection<Choice> Choices { get; set; } = new List<Choice>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("Questions")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Question")]
    public virtual ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();

    [InverseProperty("Question")]
    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
