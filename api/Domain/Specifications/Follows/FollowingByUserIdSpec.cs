using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications.Follows
{
    public class FollowingByUserIdSpec : Specification<Follow>
    {
        public FollowingByUserIdSpec(EntityId<User> userId)
        {
            Query.Where(x => x.FollowerId == userId);
        }
    }
}