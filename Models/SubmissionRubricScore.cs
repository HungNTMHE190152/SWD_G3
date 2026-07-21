using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("SubmissionGradeId", "RubricCriterionId", Name = "UQ_SubmissionRubricScores_Grade_Criterion", IsUnique = true)]
public partial class SubmissionRubricScore
{
    [Key]
    public long SubmissionRubricScoreId { get; set; }

    public long SubmissionGradeId { get; set; }

    public long RubricCriterionId { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal Score { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    [ForeignKey("RubricCriterionId")]
    [InverseProperty("SubmissionRubricScores")]
    public virtual RubricCriterion RubricCriterion { get; set; } = null!;

    [ForeignKey("SubmissionGradeId")]
    [InverseProperty("SubmissionRubricScores")]
    public virtual SubmissionGrade SubmissionGrade { get; set; } = null!;
}
