using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Follows.Common;

namespace Application.Features.Follows.GetFollowing;

public record GetFollowingQuery(int UserId) : IRequest<Result<ProfilesListResponse>>;