using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Index("RoleId", Name = "IX_Accounts_RoleId")]
[Index("UserId", Name = "UQ_Accounts_UserId", IsUnique = true)]
[Index("Username", Name = "UQ_Accounts_Username", IsUnique = true)]
public partial class Account
{
    [Key]
    public long AccountId { get; set; }

    public long UserId { get; set; }

    public long RoleId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(500)]
    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    [Precision(0)]
    public DateTime? LastLoginAt { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Accounts")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Account")]
    public virtual User User { get; set; } = null!;
}
