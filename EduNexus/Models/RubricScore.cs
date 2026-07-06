using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("RubricScore")]
[Index("ResultId", "CriterionId", Name = "UQ_RubricScore", IsUnique = true)]
public partial class RubricScore
{
    [Key]
    public long RubricScoreId { get; set; }

    public long ResultId { get; set; }

    public long CriterionId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Score { get; set; }

    [StringLength(500)]
    public string? Comment { get; set; }

    [ForeignKey("CriterionId")]
    [InverseProperty("RubricScores")]
    public virtual RubricCriterion Criterion { get; set; } = null!;

    [ForeignKey("ResultId")]
    [InverseProperty("RubricScores")]
    public virtual EssayResult Result { get; set; } = null!;
}
