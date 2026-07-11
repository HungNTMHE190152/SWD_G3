using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Lesson")]
public partial class Lesson
{
    [Key]
    public long LessonId { get; set; }

    public long ModuleId { get; set; }

    [StringLength(200)]
    public string LessonName { get; set; } = null!;

    [StringLength(30)]
    public string LessonType { get; set; } = null!;

    public string? Content { get; set; }

    [StringLength(500)]
    public string? VideoUrl { get; set; }

    public int? Duration { get; set; }

    public int DisplayOrder { get; set; }

    public bool? IsPublished { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Lesson")]
    public virtual ICollection<LessonTranscript> LessonTranscripts { get; set; } = new List<LessonTranscript>();

    [ForeignKey("ModuleId")]
    [InverseProperty("Lessons")]
    public virtual Module Module { get; set; } = null!;

    [InverseProperty("Lesson")]
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    [InverseProperty("Lesson")]
    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
