using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Enrollment")]
[Index("StudentId", Name = "IX_Enrollment_StudentId")]
public partial class Enrollment
{
    [Key]
    public long EnrollmentId { get; set; }

    public long? CourseId { get; set; }

    public long? CourseGroupId { get; set; }

    public long? ParentEnrollmentId { get; set; }

    public long StudentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EnrollDate { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Progress { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CompletedDate { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Enrollments")]
    public virtual Course? Course { get; set; }

    [ForeignKey("CourseGroupId")]
    [InverseProperty("Enrollments")]
    public virtual CourseGroup? CourseGroup { get; set; }

    [InverseProperty("ParentEnrollment")]
    public virtual ICollection<Enrollment> InverseParentEnrollment { get; set; } = new List<Enrollment>();

    [ForeignKey("ParentEnrollmentId")]
    [InverseProperty("InverseParentEnrollment")]
    public virtual Enrollment? ParentEnrollment { get; set; }

    [InverseProperty("Enrollment")]
    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    [ForeignKey("StudentId")]
    [InverseProperty("Enrollments")]
    public virtual User Student { get; set; } = null!;
}
