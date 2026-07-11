using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Models;

[Table("SystemConfig")]
[Index("ConfigKey", Name = "UQ__SystemCo__4A306784D7B6D1F3", IsUnique = true)]
public partial class SystemConfig
{
    [Key]
    public int ConfigId { get; set; }

    [StringLength(100)]
    public string ConfigKey { get; set; } = null!;

    public string? ConfigValue { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}
