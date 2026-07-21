using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("CourseId", "ReviewedAt", Name = "IX_CourseReviews_CourseId_ReviewedAt", IsDescending = new[] { false, true })]
public partial class CourseReview
{
    [Key]
    public long CourseReviewId { get; set; }

    public long CourseId { get; set; }

    public long ReviewedBy { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Decision { get; set; } = null!;

    [StringLength(1000)]
    public string? ReviewComment { get; set; }

    [Precision(0)]
    public DateTime ReviewedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("CourseReviews")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("ReviewedBy")]
    [InverseProperty("CourseReviews")]
    public virtual User ReviewedByNavigation { get; set; } = null!;
}
