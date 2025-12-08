using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserAchievement
{
    public int UserId { get; set; }

    public int AchievementId { get; set; }

    public DateTime? UnlockedAt { get; set; }

    public virtual Achievement Achievement { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
