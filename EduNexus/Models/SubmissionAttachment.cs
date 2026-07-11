using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("SubmissionAttachment")]
public partial class SubmissionAttachment
{
    [Key]
    public long AttachmentId { get; set; }

    public long SubmissionId { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [StringLength(500)]
    public string FileUrl { get; set; } = null!;

    [StringLength(50)]
    public string? FileType { get; set; }

    public long? FileSize { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UploadedAt { get; set; }

    [ForeignKey("SubmissionId")]
    [InverseProperty("SubmissionAttachments")]
    public virtual EssaySubmission Submission { get; set; } = null!;
}
