using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("ModuleId", "Status", Name = "IX_Lessons_ModuleId_Status")]
[Index("ModuleId", "DisplayOrder", Name = "UQ_Lessons_Module_DisplayOrder", IsUnique = true)]
public partial class Lesson
{
    [Key]
    public long LessonId { get; set; }

    public long ModuleId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string LessonType { get; set; } = null!;

    public int DisplayOrder { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    public int? EstimatedMinutes { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Lesson")]
    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    [InverseProperty("Lesson")]
    public virtual ICollection<LessonResource> LessonResources { get; set; } = new List<LessonResource>();

    [ForeignKey("ModuleId")]
    [InverseProperty("Lessons")]
    public virtual Module Module { get; set; } = null!;
}
