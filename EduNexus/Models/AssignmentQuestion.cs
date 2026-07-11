using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("AssignmentQuestion")]
public partial class AssignmentQuestion
{
    [Key]
    public long AssignmentQuestionId { get; set; }

    public long AssignmentId { get; set; }

    public long QuestionId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; }

    public int? DisplayOrder { get; set; }

    [ForeignKey("AssignmentId")]
    [InverseProperty("AssignmentQuestions")]
    public virtual Assignment Assignment { get; set; } = null!;

    [ForeignKey("QuestionId")]
    [InverseProperty("AssignmentQuestions")]
    public virtual Question Question { get; set; } = null!;
}
