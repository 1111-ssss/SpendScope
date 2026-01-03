using MediatR;
using Domain.Abstractions.Result;

namespace Application.Features.Follows.UnFollowUser
{
    public record UnFollowUserCommand(int UserId) : IRequest<Result>;
}