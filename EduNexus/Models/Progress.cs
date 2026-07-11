using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Progress")]
[Index("EnrollmentId", Name = "IX_Progress_EnrollmentId")]
[Index("EnrollmentId", "LessonId", Name = "UQ_Progress", IsUnique = true)]
public partial class Progress
{
    [Key]
    public long ProgressId { get; set; }

    public long EnrollmentId { get; set; }

    public long LessonId { get; set; }

    public bool? IsCompleted { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? CompletionPercentage { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastAccess { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CompletedAt { get; set; }

    [ForeignKey("EnrollmentId")]
    [InverseProperty("Progresses")]
    public virtual Enrollment Enrollment { get; set; } = null!;

    [ForeignKey("LessonId")]
    [InverseProperty("Progresses")]
    public virtual Lesson Lesson { get; set; } = null!;
}
