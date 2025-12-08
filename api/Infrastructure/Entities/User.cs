using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? Deleted { get; set; }

    public bool? IsAdmin { get; set; }

    public virtual ICollection<AppVersion> AppVersions { get; set; } = new List<AppVersion>();

    public virtual ICollection<Follow> FollowFolloweds { get; set; } = new List<Follow>();

    public virtual ICollection<Follow> FollowFollowers { get; set; } = new List<Follow>();

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
