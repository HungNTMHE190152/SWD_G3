using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Rubric")]
public partial class Rubric
{
    [Key]
    public long RubricId { get; set; }

    public long AssignmentId { get; set; }

    [StringLength(200)]
    public string RubricName { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? TotalWeight { get; set; }

    [ForeignKey("AssignmentId")]
    [InverseProperty("Rubrics")]
    public virtual Assignment Assignment { get; set; } = null!;

    [InverseProperty("Rubric")]
    public virtual ICollection<RubricCriterion> RubricCriteria { get; set; } = new List<RubricCriterion>();
}
