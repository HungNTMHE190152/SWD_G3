using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Resource")]
public partial class Resource
{
    [Key]
    public long ResourceId { get; set; }

    public long LessonId { get; set; }

    [StringLength(255)]
    public string ResourceName { get; set; } = null!;

    [StringLength(500)]
    public string FileUrl { get; set; } = null!;

    [StringLength(50)]
    public string? FileType { get; set; }

    public long? FileSize { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("Resources")]
    public virtual Lesson Lesson { get; set; } = null!;
}
