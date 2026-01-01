using MediatR;
using Domain.Abstractions.Result;

namespace Application.Features.Auth.Login
{
    public record LoginUserCommand(string Identifier, string Password) : IRequest<Result<AuthResponse>>;
}