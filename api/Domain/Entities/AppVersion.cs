using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class AppVersion : IAggregateRoot
    {
        public int Id { get; private set; }
        public string Branch { get; private set; } = default!;
        public int Build { get; private set; }
        public DateTime UploadedAt { get; private set; }
        public EntityId<User>? UploadedBy { get; private set; }
        public string? Changelog { get; private set; }

        private AppVersion() { }

        public static AppVersion Create(string branch, int build, EntityId<User> uploadedBy, string? changelog = null)
        {
            return new AppVersion
            {
                Branch = branch,
                Build = build,
                UploadedBy = uploadedBy,
                UploadedAt = DateTime.UtcNow,
                Changelog = changelog
            };
        }
    }
}