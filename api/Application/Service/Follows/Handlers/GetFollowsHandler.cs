using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;

namespace Application.Service.Follows.Handlers
{
    public class GetFollowsHandler
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<User> _users;
        private readonly IRepository<Follow> _follows;
        public GetFollowsHandler(IUnitOfWork db, IRepository<User> users, IRepository<Follow> follows)
        {
            _follows = follows;
            _db = db;
            _users = users;
        }
        public async Task<Result<List<GetProfileResponse>>> GetFollowers(EntityId<User> userId, CancellationToken ct = default)
        {
            var followers = await _follows.ListAsync(new FollowersByUserId(userId), ct);

            if (followers == null)
                return Result<List<GetProfileResponse>>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            var responseList = new List<GetProfileResponse>();

            foreach (var us in followers)
            {
                var user = await _users.GetByIdAsync(us.FollowedId, [u => u.Profile], ct);
                if (user != null)
                {
                    var profile = user.Profile;
                    if (profile != null)
                    {
                        responseList.Add(new GetProfileResponse(
                            DisplayName: profile.DisplayName ?? user.Username,
                            AvatarUrl: profile.AvatarUrl ?? "img/default-avatar.png",
                            Bio: profile.Bio ?? "",
                            LastOnline: profile.LastOnline ?? user.CreatedAt
                        ));
                    }
                }
            }

            return Result<List<GetProfileResponse>>.Success(responseList);
        }
        public async Task<Result<List<GetProfileResponse>>> GetFollowing(EntityId<User> userId, CancellationToken ct = default)
        {
            var following = await _follows.ListAsync(new FollowingByUserId(userId), ct);

            if (following == null)
                return Result<List<GetProfileResponse>>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            var responseList = new List<GetProfileResponse>();

            foreach (var us in following)
            {
                var user = await _users.GetByIdAsync(us.FollowerId, [u => u.Profile], ct);
                if (user != null)
                {
                    var profile = user.Profile;
                    if (profile != null)
                    {
                        responseList.Add(new GetProfileResponse(
                            DisplayName: profile.DisplayName ?? user.Username,
                            AvatarUrl: profile.AvatarUrl ?? "img/default-avatar.png",
                            Bio: profile.Bio ?? "",
                            LastOnline: profile.LastOnline ?? user.CreatedAt
                        ));
                    }
                }
            }

            return Result<List<GetProfileResponse>>.Success(responseList);
        }
    }
}