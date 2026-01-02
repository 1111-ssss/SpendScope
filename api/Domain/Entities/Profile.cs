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
        public static Profile Create()
        {
            return new Profile();
        }
        public void Update(string? displayName = null, string? bio = null, string? avatarUrl = null)
        {
            if (displayName != null)
                DisplayName = displayName;
            if (bio != null)
                Bio = bio;
            if (avatarUrl != null)
                AvatarUrl = avatarUrl;
            LastOnline = DateTime.UtcNow;
        }
        public void UpdateLastOnline()
        {
            LastOnline = DateTime.UtcNow;
        }
    }
}