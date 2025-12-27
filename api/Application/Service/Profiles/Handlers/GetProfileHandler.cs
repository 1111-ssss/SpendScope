using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;

namespace Application.Service.Profiles.Handlers
{
    public class GetProfileHandler
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<User> _users;
        public GetProfileHandler(IUnitOfWork db, IRepository<User> users)
        {
            _db = db;
            _users = users;
        }
        public async Task<Result<GetProfileResponse>> Handle(EntityId<User> userId, CancellationToken ct = default)
        {
            var user = await _users.GetByIdAsync(userId, [u => u.Profile], ct);

            if (user == null)
                return Result<GetProfileResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            var profile = user.Profile;
            if (profile == null)
                return Result<GetProfileResponse>.Failed(ErrorCode.NotFound, "Профиль пользователя не найден");

            return Result<GetProfileResponse>.Success(new GetProfileResponse(
                DisplayName: profile.DisplayName ?? user.Username,
                AvatarUrl: profile.AvatarUrl ?? "img/default-avatar.png",
                Bio: profile.Bio ?? "",
                LastOnline: profile.LastOnline ?? user.CreatedAt
            ));
        }
    }
}