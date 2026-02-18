using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Profiles;

public class GetProfilesByNameSpec : Specification<User>
{
    public GetProfilesByNameSpec(string username, int page, int pageSize)
    {
        if (!string.IsNullOrWhiteSpace(username))
        {
            username = username.Trim();
            Query
                .Include(x => x.Profile)
                .Where(
                    l => l.Username.Contains(username)

                    || !string.IsNullOrEmpty(l.Profile.DisplayName)
                    && l.Profile.DisplayName.Contains(username)
                );
        }
        else
        {
            Query
                .Include(x => x.Profile);
        }

        Query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}