using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications.Follows;

public class UsersWithProfileByIdsSpec : Specification<User>
{
    public UsersWithProfileByIdsSpec(List<EntityId<User>> userIds)
    {
        Query.Where(x => userIds.Contains(x.Id))
            .Include(u => u.Profile);
    }
}