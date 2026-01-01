using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications
{
    public class FollowingByUserId : Specification<Follow>
    {
        public FollowingByUserId(EntityId<User> userId)
        {
            Query.Where(x => x.FollowerId == userId);
        }
    }
}