using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications.Auth;

public class RefreshTokenByUserSpec : Specification<RefreshToken>
{
    public RefreshTokenByUserSpec(string token, EntityId<User> userId)
    {
        Query
            .Where(t => t.Token == token && t.UserId == userId);
    }
    public RefreshTokenByUserSpec(EntityId<User> userId)
    {
        Query
            .Where(t => t.UserId == userId);
    }
}