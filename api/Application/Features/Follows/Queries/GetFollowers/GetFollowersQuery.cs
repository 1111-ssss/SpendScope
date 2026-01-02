using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Follows.Common;

namespace Application.Features.Follows.GetFollowers
{
    public record GetFollowersQuery(int UserId) : IRequest<Result<ProfilesListResponse>>;
}