using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications
{
    public class FollowersByUserId : Specification<Follow>
    {
        public FollowersByUserId(EntityId<User> userId)
        {
            Query.Where(x => x.FollowedId == userId);
        }
    }
}