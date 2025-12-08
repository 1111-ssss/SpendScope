using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Profile
{
    public int UserId { get; set; }

    public string? DisplayName { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Bio { get; set; }

    public DateTime? LastOnline { get; set; }

    public virtual User User { get; set; } = null!;
}
