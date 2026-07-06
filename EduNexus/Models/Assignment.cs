using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Assignment")]
public partial class Assignment
{
    [Key]
    public long AssignmentId { get; set; }

    public long ModuleId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(30)]
    public string AssignmentType { get; set; } = null!;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? TotalScore { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OpenDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DueDate { get; set; }

    public bool? AllowLateSubmission { get; set; }

    [Column("IsAIGenerated")]
    public bool? IsAigenerated { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Assignment")]
    public virtual ICollection<AssignmentQuestion> AssignmentQuestions { get; set; } = new List<AssignmentQuestion>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("Assignments")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Assignment")]
    public virtual ICollection<EssaySubmission> EssaySubmissions { get; set; } = new List<EssaySubmission>();

    [ForeignKey("ModuleId")]
    [InverseProperty("Assignments")]
    public virtual Module Module { get; set; } = null!;

    [InverseProperty("Assignment")]
    public virtual Quiz? Quiz { get; set; }

    [InverseProperty("Assignment")]
    public virtual ICollection<Rubric> Rubrics { get; set; } = new List<Rubric>();
}
