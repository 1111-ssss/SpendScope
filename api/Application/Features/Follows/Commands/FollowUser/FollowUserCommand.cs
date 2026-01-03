using MediatR;
using Domain.Abstractions.Result;

namespace Application.Features.Follows.FollowUser
{
    public record FollowUserCommand(int UserId) : IRequest<Result>;
}