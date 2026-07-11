using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("RubricCriterion")]
public partial class RubricCriterion
{
    [Key]
    public long CriterionId { get; set; }

    public long RubricId { get; set; }

    [StringLength(200)]
    public string CriterionName { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Weight { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal MaxScore { get; set; }

    public int? DisplayOrder { get; set; }

    [ForeignKey("RubricId")]
    [InverseProperty("RubricCriteria")]
    public virtual Rubric Rubric { get; set; } = null!;

    [InverseProperty("Criterion")]
    public virtual ICollection<RubricScore> RubricScores { get; set; } = new List<RubricScore>();
}
