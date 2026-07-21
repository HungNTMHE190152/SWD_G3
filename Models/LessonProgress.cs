using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("LessonProgress")]
[Index("EnrollmentId", "IsCompleted", Name = "IX_LessonProgress_EnrollmentId_IsCompleted")]
[Index("EnrollmentId", "LessonId", Name = "UQ_LessonProgress_Enrollment_Lesson", IsUnique = true)]
public partial class LessonProgress
{
    [Key]
    public long LessonProgressId { get; set; }

    public long EnrollmentId { get; set; }

    public long LessonId { get; set; }

    public bool IsCompleted { get; set; }

    [Precision(0)]
    public DateTime? StartedAt { get; set; }

    [Precision(0)]
    public DateTime? CompletedAt { get; set; }

    [Precision(0)]
    public DateTime? LastAccessedAt { get; set; }

    [ForeignKey("EnrollmentId")]
    [InverseProperty("LessonProgresses")]
    public virtual Enrollment Enrollment { get; set; } = null!;

    [ForeignKey("LessonId")]
    [InverseProperty("LessonProgresses")]
    public virtual Lesson Lesson { get; set; } = null!;
}
