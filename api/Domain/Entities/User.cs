using Domain.ValueObjects;
using Domain.Abstractions.Interfaces;

namespace Domain.Entities
{
    public class User : IAggregateRoot
    {
        public EntityId<User> Id { get; private set; } = EntityId<User>.Empty;
        public string Username { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; }
        public bool Deleted { get; private set; } = false;
        public bool IsAdmin { get; private set; } = false;

        public Profile Profile { get; private set; } = default!;
        private readonly List<EntityId<User>> _followerIds = new();
        private readonly List<EntityId<User>> _followedIds = new();
        public IReadOnlyList<EntityId<User>> FollowerIds => _followerIds.AsReadOnly();
        public IReadOnlyList<EntityId<User>> FollowedIds => _followedIds.AsReadOnly();

        private readonly List<int> _unlockedAchievementIds = new();
        public IReadOnlyList<int> UnlockedAchievementIds => _unlockedAchievementIds.AsReadOnly();

        private User() { }
        public static User Create(string username, string email, string passwordHash, bool isAdmin = false)
        {
            var user = new User
            {
                Username = username.Trim(),
                Email = email.Trim(),
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                IsAdmin = isAdmin,
                Profile = Profile.Create()
            };
            return user;
        }
        public void UpdateProfile(string? displayName, string? avatarUrl, string? bio)
        {
            Profile.DisplayName = displayName;
            Profile.AvatarUrl = avatarUrl;
            Profile.Bio = bio;
            Profile.LastOnline = DateTime.UtcNow;
        }

        public void Follow(EntityId<User> followedId)
        {
            if (!_followedIds.Contains(followedId))
                _followedIds.Add(followedId);
        }

        public void Unfollow(EntityId<User> followedId)
        {
            _followedIds.Remove(followedId);
        }

        public void UnlockAchievement(int achievementId)
        {
            if (!_unlockedAchievementIds.Contains(achievementId))
                _unlockedAchievementIds.Add(achievementId);
        }

        public void MarkAsDeleted() => Deleted = true;
    }
}