using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Auth;

public class UserByEmailSpec : Specification<User>
{
    public UserByEmailSpec(string email)
    {
        Query.Where(e =>
            e.Email == email
        );
    }
}