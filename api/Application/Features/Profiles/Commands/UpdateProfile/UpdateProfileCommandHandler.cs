using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Application.Features.Profiles.Common;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.UpdateProfile
{

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<ProfileResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Profile> _profileRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IFileStorage _fileStorage;
        private readonly IImageFormatter _imageFormatter;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpdateProfileCommandHandler> _logger;
        public UpdateProfileCommandHandler(
            IUnitOfWork uow,
            IBaseRepository<Profile> profileRepository,
            IBaseRepository<User> userRepository,
            IFileStorage fileStorage,
            IImageFormatter imageFormatter,
            ICurrentUserService currentUserService,
            ILogger<UpdateProfileCommandHandler> logger)
        {
            _uow = uow;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _fileStorage = fileStorage;
            _imageFormatter = imageFormatter;
            _currentUserService = currentUserService;
            _logger = logger;
        }
        public async Task<Result<ProfileResponse>> Handle(UpdateProfileCommand request, CancellationToken ct)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
                return Result<ProfileResponse>.Failed(ErrorCode.Unauthorized, "Не удалось определить пользователя");

            var user = await _userRepository.GetByIdAsync(userId.Value, ct);

            if (user == null)
                return Result<ProfileResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            // var profile = await _profileRepository.GetByIdAsync(user.Id, ct);
            var profile = user.Profile;

            if (profile == null)
                return Result<ProfileResponse>.Failed(ErrorCode.NotFound, "Профиль пользователя не найден");

            _logger.LogInformation($"User: {user.Username}, Profile: {profile}");

            var avatarPath = _fileStorage.GetFilePath(profile.AvatarUrl);
            if (avatarPath == null && request.Image != null)
            {
                profile.Update(avatarUrl: $"avatars/{Guid.NewGuid()}.png");
            }

            profile.Update(displayName: request.DisplayName, bio: request.Bio);

            await _profileRepository.UpdateAsync(profile, ct);

            try
            {
                await _uow.SaveChangesAsync(ct);

                var response = new ProfileResponse(
                    DisplayName: profile.DisplayName ?? user.Username,
                    Bio: profile.Bio ?? "",
                    AvatarUrl: profile.AvatarUrl ?? "avatars/default-avatar.png",
                    LastOnline: profile.LastOnline ?? user.CreatedAt
                );

                if (request.Image != null)
                {
                    var result = await _imageFormatter.FormatImage(request.Image, profile.AvatarUrl!, ct);
                    return result.Bind(() => response);
                }
                return Result<ProfileResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении достижения");
                return Result<ProfileResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при обновлении достижения");
            }
        }
    }
}