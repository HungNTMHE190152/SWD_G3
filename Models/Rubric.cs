using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("AssignmentTemplateId", Name = "UQ_Rubrics_AssignmentTemplate", IsUnique = true)]
public partial class Rubric
{
    [Key]
    public long RubricId { get; set; }

    public long AssignmentTemplateId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("AssignmentTemplateId")]
    [InverseProperty("Rubric")]
    public virtual AssignmentTemplate AssignmentTemplate { get; set; } = null!;

    [InverseProperty("Rubric")]
    public virtual ICollection<RubricCriterion> RubricCriteria { get; set; } = new List<RubricCriterion>();
}
