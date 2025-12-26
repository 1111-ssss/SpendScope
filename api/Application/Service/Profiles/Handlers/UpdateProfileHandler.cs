using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Application.Service.Profiles.Helpers;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;
using Logger;

namespace Application.Service.Profiles.Handlers
{
    public class UpdateProfileHandler
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<User> _users;
        private readonly IRepository<Profile> _profiles;
        private readonly ICustomLogger<UpdateProfileHandler> _logger;
        private readonly ProfileValidator _profileValidator;
        public UpdateProfileHandler(ProfileValidator profileValidator, ICustomLogger<UpdateProfileHandler> logger, IUnitOfWork db, IRepository<User> users, IRepository<Profile> profiles)
        {
            _profileValidator = profileValidator;
            _profiles = profiles;
            _logger = logger;
            _db = db;
            _users = users;
        }
        public async Task<Result<GetProfileResponse>> Handle(UpdateProfileRequest request, EntityId<User> userId, CancellationToken ct = default)
        {
            var user = await _users.GetByIdAsync(userId, ct);
            if (user == null)
                return Result<GetProfileResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            var profile = user.Profile;
            if (profile == null)
                return Result<GetProfileResponse>.Failed(ErrorCode.NotFound, "Профиль пользователя не найден");

            var displayName = _profileValidator.ValidateDisplayName(request.DisplayName);
            if (!displayName.IsSuccess)
                return Result<GetProfileResponse>.Failed(ErrorCode.BadRequest, displayName.Message ?? "Недопустимое имя");
            
            var bio = _profileValidator.ValidateBio(request.Bio);
            if (!bio.IsSuccess)
                return Result<GetProfileResponse>.Failed(ErrorCode.BadRequest, bio.Message ?? "Недопустимое имя");

            profile.Update(displayName.Value, bio.Value);

            await _profiles.UpdateAsync(profile, ct);
            try {
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при сохранении профиля", ex);
                return Result<GetProfileResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при сохранении профиля");
            }

            return Result<GetProfileResponse>.Success(new GetProfileResponse(
                DisplayName: profile.DisplayName ?? user.Username,
                AvatarUrl: profile.AvatarUrl ?? "img/default-avatar.png",
                Bio: profile.Bio ?? "",
                LastOnline: profile.LastOnline ?? user.CreatedAt
            ));
        }
    }
}