using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications.Follows;

public class FollowersByUserIdSpec : Specification<Follow>
{
    public FollowersByUserIdSpec(EntityId<User> userId)
    {
        Query.Where(x => x.FollowedId == userId);
    }
}