using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Achievement : IAggregateRoot
{
    public EntityId<Achievement> Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? IconUrl { get; private set; }

    private readonly List<UserAchievement> _userAchievements = new();
    public IReadOnlyList<UserAchievement> UserAchievements => _userAchievements.AsReadOnly();

    private Achievement() { }

    public static Achievement Create(string name, string? description = null, string? iconUrl = null)
    {
        return new Achievement
        {
            Name = name,
            Description = description,
            IconUrl = iconUrl
        };
    }
    public void Update(string? name = null, string? description = null, string? iconUrl = null)
    {
        if (name != null)
            Name = name;
        if (description != null)
            Description = description;
        if (iconUrl != null)
            IconUrl = iconUrl;
    }
    public void UnlockForUser(EntityId<User> userId, DateTime unlockedAt)
    {
        if (_userAchievements.All(ua => ua.UserId != userId))
        {
            _userAchievements.Add(new UserAchievement
            {
                UserId = userId,
                AchievementId = Id,
                UnlockedAt = unlockedAt
            });
        }
    }
}
public class UserAchievement
{
    public EntityId<User> UserId { get; init; }
    public int AchievementId { get; init; }
    public DateTime? UnlockedAt { get; init; }
}