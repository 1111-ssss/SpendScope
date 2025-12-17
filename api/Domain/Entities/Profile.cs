using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Profile : IAggregateRoot
    {
        public EntityId<User> UserId { get; private set; }
        public string? DisplayName { get; internal set; }
        public string? AvatarUrl { get; internal set; }
        public string? Bio { get; internal set; }
        public DateTime? LastOnline { get; internal set; }
        private Profile() { }
        public static Profile Create(EntityId<User> userId)
        {
            return new Profile { UserId = userId };
        }
        public void Update(string? displayName, string? avatarUrl, string? bio)
        {
            DisplayName = displayName;
            AvatarUrl = avatarUrl;
            Bio = bio;
            LastOnline = DateTime.UtcNow;
        }
    }
}