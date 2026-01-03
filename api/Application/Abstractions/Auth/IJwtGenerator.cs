using Application.Features.Auth;
using Domain.Abstractions.Result;
using Domain.Entities;

namespace Application.Abstractions.Auth
{
    public interface IJwtGenerator
    {
        Result<AuthResponse> GenerateToken(User user);
    }
}