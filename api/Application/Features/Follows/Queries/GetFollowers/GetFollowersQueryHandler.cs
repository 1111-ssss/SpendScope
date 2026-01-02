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

            if (followers == null)
                return Result<ProfilesListResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            var responseList = new List<ProfileResponse>();

            foreach (var us in followers)
            {
                var user = await _userRepository.GetByIdAsync(us.FollowedId, ct);
                if (user != null)
                {
                    var profile = user.Profile;
                    if (profile != null)
                    {
                        responseList.Add(new ProfileResponse(
                            DisplayName: profile.DisplayName ?? user.Username,
                            AvatarUrl: profile.AvatarUrl ?? "avatars/default-avatar.png",
                            Bio: profile.Bio ?? "",
                            LastOnline: profile.LastOnline ?? user.CreatedAt
                        ));
                    }
                }
            }

            return Result<ProfilesListResponse>.Success(new ProfilesListResponse(
                Profiles: responseList.ToArray()
            ));
        }
    }
}