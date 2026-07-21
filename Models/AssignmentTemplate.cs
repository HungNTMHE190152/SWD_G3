using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("CourseId", "IsActive", Name = "IX_AssignmentTemplates_CourseId_IsActive")]
public partial class AssignmentTemplate
{
    [Key]
    public long AssignmentTemplateId { get; set; }

    public long CourseId { get; set; }

    public long? ModuleId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public string? Instructions { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal TotalScore { get; set; }

    public bool IsActive { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("AssignmentTemplate")]
    public virtual ICollection<ClassroomAssignment> ClassroomAssignments { get; set; } = new List<ClassroomAssignment>();

    [ForeignKey("CourseId")]
    [InverseProperty("AssignmentTemplates")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("AssignmentTemplates")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [ForeignKey("ModuleId")]
    [InverseProperty("AssignmentTemplates")]
    public virtual Module? Module { get; set; }

    [InverseProperty("AssignmentTemplate")]
    public virtual Rubric? Rubric { get; set; }
}
