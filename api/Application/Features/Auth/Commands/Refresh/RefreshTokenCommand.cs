using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Auth.Refresh;

public record RefreshTokenCommand(string JwtToken, string RefreshToken) : IRequest<Result<AuthResponse>>;