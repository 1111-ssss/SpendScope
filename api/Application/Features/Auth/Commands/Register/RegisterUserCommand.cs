using MediatR;
using Domain.Abstractions.Result;

namespace Application.Features.Auth.Register
{
    public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<Result<AuthResponse>>;
}