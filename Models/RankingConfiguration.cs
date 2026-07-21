using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

public partial class RankingConfiguration
{
    [Key]
    public long RankingConfigurationId { get; set; }

    [Column(TypeName = "decimal(6, 5)")]
    public decimal AssignmentWeight { get; set; }

    [Column(TypeName = "decimal(6, 5)")]
    public decimal QuizWeight { get; set; }

    [Column(TypeName = "decimal(6, 5)")]
    public decimal LessonProgressWeight { get; set; }

    public bool IsActive { get; set; }

    public long UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("UpdatedBy")]
    [InverseProperty("RankingConfigurations")]
    public virtual User UpdatedByNavigation { get; set; } = null!;
}
