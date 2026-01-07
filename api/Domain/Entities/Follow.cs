using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Follow : IAggregateRoot
{
    public EntityId<User> FollowerId { get; private set; }
    public EntityId<User> FollowedId { get; private set; }
    public DateTime FollowedAt { get; private set; }

    private Follow() { }

    public static Follow Create(EntityId<User> followerId, EntityId<User> followedId)
    {
        return new Follow
        {
            FollowerId = followerId,
            FollowedId = followedId,
            FollowedAt = DateTime.UtcNow
        };
    }
}