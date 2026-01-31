using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Features.Follows.Common;
using Application.Features.Profiles.Common;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Follows;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Follows.GetFollowing;

public class GetFollowingQueryHandler : IRequestHandler<GetFollowingQuery, Result<ProfilesListResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Follow> _followRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly ILogger<GetFollowingQueryHandler> _logger;
    public GetFollowingQueryHandler(
        IUnitOfWork uow,
        IBaseRepository<Follow> followRepository,
        IBaseRepository<User> userRepository,
        ILogger<GetFollowingQueryHandler> logger)
    {
        _uow = uow;
        _followRepository = followRepository;
        _userRepository = userRepository;
        _logger = logger;
    }
    public async Task<Result<ProfilesListResponse>> Handle(GetFollowingQuery request, CancellationToken ct)
    {
        var following = await _followRepository.ListAsync(new FollowingByUserIdSpec(request.UserId), ct);

        if (!following.Any())
            return Result<ProfilesListResponse>.Success(new ProfilesListResponse(Profiles: Array.Empty<ProfileResponse>()));

        var followerIds = following.Select(f => f.FollowerId).ToList();

        var users = await _userRepository.ListAsync(new UsersWithProfileByIdsSpec(followerIds), ct);

        var profiles = users
            .Where(u => u.Profile != null)
            .Select(u => new ProfileResponse(
                DisplayName: u.Profile.DisplayName ?? u.Username,
                Username: u.Username,
                AvatarUrl: u.Profile.AvatarUrl ?? "avatars/default-avatar.png",
                Bio: u.Profile.Bio ?? "",
                LastOnline: u.Profile.LastOnline ?? u.CreatedAt
            ))
            .ToArray();

        return Result<ProfilesListResponse>.Success(new ProfilesListResponse(
            Profiles: profiles
        ));
    }
}