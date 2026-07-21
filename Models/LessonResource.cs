using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

public partial class LessonResource
{
    [Key]
    public long LessonResourceId { get; set; }

    public long LessonId { get; set; }

    [StringLength(200)]
    public string ResourceName { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string ResourceType { get; set; } = null!;

    [StringLength(1000)]
    public string ResourceUrl { get; set; } = null!;

    public int DisplayOrder { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("LessonResources")]
    public virtual Lesson Lesson { get; set; } = null!;
}
