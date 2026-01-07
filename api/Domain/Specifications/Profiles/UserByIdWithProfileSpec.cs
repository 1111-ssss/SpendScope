using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications.Profiles;

public class UserByIdWithProfileSpec : Specification<User>
{
    public UserByIdWithProfileSpec(EntityId<User> userId)
    {
        Query.Where(x => x.Id == userId)
            .Include(x => x.Profile);
    }
}