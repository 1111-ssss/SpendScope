using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class AppVersion
{
    public int Id { get; set; }

    public string Branch { get; set; } = null!;

    public int Build { get; set; }

    public DateTime? UploadedAt { get; set; }

    public int? UploadedBy { get; set; }

    public string? Changelog { get; set; }

    public virtual User? UploadedByNavigation { get; set; }
}
