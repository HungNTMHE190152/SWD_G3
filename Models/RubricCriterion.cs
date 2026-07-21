using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("RubricId", "DisplayOrder", Name = "UQ_RubricCriteria_Rubric_Order", IsUnique = true)]
public partial class RubricCriterion
{
    [Key]
    public long RubricCriterionId { get; set; }

    public long RubricId { get; set; }

    [StringLength(200)]
    public string CriterionName { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal MaxScore { get; set; }

    public int DisplayOrder { get; set; }

    [ForeignKey("RubricId")]
    [InverseProperty("RubricCriteria")]
    public virtual Rubric Rubric { get; set; } = null!;

    [InverseProperty("RubricCriterion")]
    public virtual ICollection<SubmissionRubricScore> SubmissionRubricScores { get; set; } = new List<SubmissionRubricScore>();
}
