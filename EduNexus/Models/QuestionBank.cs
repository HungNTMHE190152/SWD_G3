using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("QuestionBank")]
public partial class QuestionBank
{
    [Key]
    public long BankId { get; set; }

    public long CourseId { get; set; }

    public long CreatedBy { get; set; }

    [StringLength(200)]
    public string BankName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsPublic { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("QuestionBanks")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("QuestionBanks")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Bank")]
    public virtual ICollection<QuestionImportHistory> QuestionImportHistories { get; set; } = new List<QuestionImportHistory>();

    [InverseProperty("Bank")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
