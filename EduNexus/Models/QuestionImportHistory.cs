using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("QuestionImportHistory")]
public partial class QuestionImportHistory
{
    [Key]
    public long ImportId { get; set; }

    public long BankId { get; set; }

    public long ImportedBy { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = null!;

    public int? TotalQuestions { get; set; }

    public int? SuccessQuestions { get; set; }

    public int? FailedQuestions { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ImportedAt { get; set; }

    [ForeignKey("BankId")]
    [InverseProperty("QuestionImportHistories")]
    public virtual QuestionBank Bank { get; set; } = null!;

    [ForeignKey("ImportedBy")]
    [InverseProperty("QuestionImportHistories")]
    public virtual User ImportedByNavigation { get; set; } = null!;
}
