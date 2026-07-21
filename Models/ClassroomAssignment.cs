using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("ClassroomId", "Status", Name = "IX_ClassroomAssignments_ClassroomId_Status")]
public partial class ClassroomAssignment
{
    [Key]
    public long ClassroomAssignmentId { get; set; }

    public long ClassroomId { get; set; }

    public long AssignmentTemplateId { get; set; }

    public long PublishedBy { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Precision(0)]
    public DateTime OpenDate { get; set; }

    [Precision(0)]
    public DateTime DueDate { get; set; }

    public bool AllowLateSubmission { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(0)]
    public DateTime? PublishedAt { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("ClassroomAssignment")]
    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    [ForeignKey("AssignmentTemplateId")]
    [InverseProperty("ClassroomAssignments")]
    public virtual AssignmentTemplate AssignmentTemplate { get; set; } = null!;

    [ForeignKey("ClassroomId")]
    [InverseProperty("ClassroomAssignments")]
    public virtual Classroom Classroom { get; set; } = null!;

    [ForeignKey("PublishedBy")]
    [InverseProperty("ClassroomAssignments")]
    public virtual User PublishedByNavigation { get; set; } = null!;
}
