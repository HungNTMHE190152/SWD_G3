using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("CourseId", Name = "IX_Modules_CourseId")]
[Index("CourseId", "DisplayOrder", Name = "UQ_Modules_Course_DisplayOrder", IsUnique = true)]
public partial class Module
{
    [Key]
    public long ModuleId { get; set; }

    public long CourseId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Module")]
    public virtual ICollection<AssignmentTemplate> AssignmentTemplates { get; set; } = new List<AssignmentTemplate>();

    [ForeignKey("CourseId")]
    [InverseProperty("Modules")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Module")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [InverseProperty("Module")]
    public virtual ICollection<QuizTemplate> QuizTemplates { get; set; } = new List<QuizTemplate>();
}
