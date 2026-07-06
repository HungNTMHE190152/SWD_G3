using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("Account")]
[Index("Username", Name = "UQ__Account__536C85E4B64B7EE1", IsUnique = true)]
public partial class Account
{
    [Key]
    public long AccountId { get; set; }

    public long UserId { get; set; }

    public int RoleId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? PasswordHash { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Provider { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ProviderUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    public bool? IsVerified { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Accounts")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Accounts")]
    public virtual User User { get; set; } = null!;
}
