using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications.Follows;

public class FollowExistsSpec : Specification<Follow>
{
    public FollowExistsSpec(EntityId<User> followerId, EntityId<User> followedId)
    {
        Query.Where(f => f.FollowerId == followerId && f.FollowedId == followedId);
    }
}