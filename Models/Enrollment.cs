using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("ClassroomId", "Status", Name = "IX_Enrollments_ClassroomId_Status")]
[Index("StudentId", "Status", Name = "IX_Enrollments_StudentId_Status")]
[Index("ClassroomId", "StudentId", Name = "UQ_Enrollments_Classroom_Student", IsUnique = true)]
public partial class Enrollment
{
    [Key]
    public long EnrollmentId { get; set; }

    public long ClassroomId { get; set; }

    public long StudentId { get; set; }

    [Precision(0)]
    public DateTime EnrolledAt { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(0)]
    public DateTime? CompletedAt { get; set; }

    [ForeignKey("ClassroomId")]
    [InverseProperty("Enrollments")]
    public virtual Classroom Classroom { get; set; } = null!;

    [InverseProperty("Enrollment")]
    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    [ForeignKey("StudentId")]
    [InverseProperty("Enrollments")]
    public virtual User Student { get; set; } = null!;
}
