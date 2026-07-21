using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("CourseId", "Status", Name = "IX_Classrooms_CourseId_Status")]
[Index("TeacherId", "Status", Name = "IX_Classrooms_TeacherId_Status")]
[Index("EnrollmentCode", Name = "UQ_Classrooms_EnrollmentCode", IsUnique = true)]
public partial class Classroom
{
    [Key]
    public long ClassroomId { get; set; }

    public long CourseId { get; set; }

    public long TeacherId { get; set; }

    [StringLength(200)]
    public string ClassName { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string EnrollmentCode { get; set; } = null!;

    [Precision(0)]
    public DateTime StartDate { get; set; }

    [Precision(0)]
    public DateTime EndDate { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Classroom")]
    public virtual ICollection<ClassroomAssignment> ClassroomAssignments { get; set; } = new List<ClassroomAssignment>();

    [InverseProperty("Classroom")]
    public virtual ICollection<ClassroomQuiz> ClassroomQuizzes { get; set; } = new List<ClassroomQuiz>();

    [ForeignKey("CourseId")]
    [InverseProperty("Classrooms")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Classroom")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [ForeignKey("TeacherId")]
    [InverseProperty("Classrooms")]
    public virtual User Teacher { get; set; } = null!;

    [InverseProperty("Classroom")]
    public virtual ICollection<TeacherEvaluation> TeacherEvaluations { get; set; } = new List<TeacherEvaluation>();
}
