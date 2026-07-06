using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("CourseGroup")]
public partial class CourseGroup
{
    [Key]
    public long CourseGroupId { get; set; }

    [StringLength(150)]
    public string GroupName { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CourseGroups")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("CourseGroup")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [ForeignKey("CourseGroupId")]
    [InverseProperty("CourseGroups")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
