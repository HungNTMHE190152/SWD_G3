using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("CategoryId", "Status", Name = "IX_Courses_CategoryId_Status")]
[Index("CreatedBy", "Status", Name = "IX_Courses_CreatedBy_Status")]
[Index("CourseCode", Name = "UQ_Courses_CourseCode", IsUnique = true)]
public partial class Course
{
    [Key]
    public long CourseId { get; set; }

    public long CategoryId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string CourseCode { get; set; } = null!;

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [Precision(0)]
    public DateTime? SubmittedAt { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<AssignmentTemplate> AssignmentTemplates { get; set; } = new List<AssignmentTemplate>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Courses")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("Courses")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    [InverseProperty("Course")]
    public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = new List<QuestionBank>();

    [InverseProperty("Course")]
    public virtual ICollection<QuizTemplate> QuizTemplates { get; set; } = new List<QuizTemplate>();
}
