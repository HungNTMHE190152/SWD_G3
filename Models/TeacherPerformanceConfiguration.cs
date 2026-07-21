using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

public partial class TeacherPerformanceConfiguration
{
    [Key]
    public long TeacherPerformanceConfigurationId { get; set; }

    [Column(TypeName = "decimal(6, 5)")]
    public decimal FeedbackWeight { get; set; }

    [Column(TypeName = "decimal(6, 5)")]
    public decimal CompletionWeight { get; set; }

    [Column(TypeName = "decimal(6, 5)")]
    public decimal StudentPerformanceWeight { get; set; }

    public int MinimumEvaluationCount { get; set; }

    public bool IsActive { get; set; }

    public long UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("UpdatedBy")]
    [InverseProperty("TeacherPerformanceConfigurations")]
    public virtual User UpdatedByNavigation { get; set; } = null!;
}
