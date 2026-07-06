using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Course")]
[Index("CourseCode", Name = "UQ__Course__FC00E000F235187D", IsUnique = true)]
public partial class Course
{
    [Key]
    public long CourseId { get; set; }

    public int CategoryId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string CourseCode { get; set; } = null!;

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(300)]
    public string? Thumbnail { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [StringLength(30)]
    public string? Level { get; set; }

    [StringLength(30)]
    public string? Language { get; set; }

    public int? Duration { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PublishedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Courses")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("Courses")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Course")]
    public virtual ICollection<FlashcardSet> FlashcardSets { get; set; } = new List<FlashcardSet>();

    [InverseProperty("Course")]
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    [InverseProperty("Course")]
    public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = new List<QuestionBank>();

    [ForeignKey("CourseId")]
    [InverseProperty("Courses")]
    public virtual ICollection<CourseGroup> CourseGroups { get; set; } = new List<CourseGroup>();
}
