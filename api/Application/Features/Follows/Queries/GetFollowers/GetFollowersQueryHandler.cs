using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Features.Follows.Common;
using Application.Features.Profiles.Common;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Follows;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Follows.GetFollowers
{
    public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, Result<ProfilesListResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly ILogger<GetFollowersQueryHandler> _logger;
        public GetFollowersQueryHandler(
            IUnitOfWork uow,
            IBaseRepository<Follow> followRepository,
            IBaseRepository<User> userRepository,
            ILogger<GetFollowersQueryHandler> logger)
        {
            _uow = uow;
            _followRepository = followRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<Result<ProfilesListResponse>> Handle(GetFollowersQuery request, CancellationToken ct)
        {
            var followers = await _followRepository.ListAsync(new FollowersByUserIdSpec(request.UserId), ct);

            if (!followers.Any())
            {
                return Result<ProfilesListResponse>.Success(new ProfilesListResponse(Profiles: Array.Empty<ProfileResponse>()));
            }

            var followerIds = followers.Select(f => f.FollowerId).ToList();

            var users = await _userRepository.ListAsync(new UsersWithProfileByIdsSpec(followerIds), ct);

            var profiles = users
                .Where(u => u.Profile != null)
                .Select(u => new ProfileResponse(
                    DisplayName: u.Profile.DisplayName ?? u.Username,
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
}