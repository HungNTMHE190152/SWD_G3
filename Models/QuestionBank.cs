using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("CourseId", Name = "IX_QuestionBanks_CourseId")]
public partial class QuestionBank
{
    [Key]
    public long QuestionBankId { get; set; }

    public long CourseId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(200)]
    public string BankName { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsPublic { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("QuestionBanks")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("QuestionBanks")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("QuestionBank")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
