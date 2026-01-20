using admin.Features.Auth.DTO.Requests;
using admin.Features.Auth.DTO.Responses;
using Refit;

namespace admin.Infrastructure.Http.Clients;

public interface IAuthApi
{
    [Post("/auth/register")]
    Task<AuthResponse> Register([Body] RegisterRequest request, CancellationToken ct = default!);

    [Post("/auth/login")]
    Task<AuthResponse> Login([Body] LoginRequest request, CancellationToken ct = default!);

    [Post("/auth/refresh")]
    Task<AuthResponse> RefreshToken([Body] RefreshTokenRequest request, CancellationToken ct = default!);
}
