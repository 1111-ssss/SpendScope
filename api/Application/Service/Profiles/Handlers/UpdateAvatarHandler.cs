using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Application.Service.Profiles.Handlers
{
    public class UpdateAvatarHandler
    {
        private readonly ICustomLogger<UpdateAvatarHandler> _logger;
        private readonly IUnitOfWork _db;
        private readonly IRepository<User> _users;
        private readonly IRepository<Profile> _profiles;
        public UpdateAvatarHandler(ICustomLogger<UpdateAvatarHandler> logger, IUnitOfWork db, IRepository<User> users, IRepository<Profile> profiles)
        {
            _profiles = profiles;
            _logger = logger;
            _db = db;
            _users = users;
        }
        public async Task<Result<UpdateAvatarResponse>> Handle(EntityId<User> userId, IFormFile file, string avatarPath, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                return Result<UpdateAvatarResponse>.Failed(ErrorCode.BadRequest, "Файл не загружен");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                return Result<UpdateAvatarResponse>.Failed(ErrorCode.BadRequest, "Файл должен иметь расширение .png, .jpg или .jpeg");

            var user = await _users.GetByIdAsync(userId, [u => u.Profile], ct);
            if (user == null)
                return Result<UpdateAvatarResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            if (!Directory.Exists(avatarPath))
                Directory.CreateDirectory(avatarPath);

            var avatarFilePath = Path.Combine(avatarPath, $"{userId}.png");
            try
            {
                using var image = await Image.LoadAsync(file.OpenReadStream(), ct);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(512, 512),
                    Mode = ResizeMode.Max
                }));

                await image.SaveAsPngAsync(avatarFilePath, ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при обработке изображения аватара", ex);
                return Result<UpdateAvatarResponse>.Failed(ErrorCode.BadRequest, "Невалидное или повреждённое изображение");
            }
            
            var profile = user.Profile;
            if (profile == null)
                return Result<UpdateAvatarResponse>.Failed(ErrorCode.NotFound, "Профиль пользователя не найден");
            
            profile.UpdateAvatar($"/avatars/{userId}.png");

            await _profiles.UpdateAsync(profile, ct);

            try {
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при сохранении профиля в базе данных", ex);
                return Result<UpdateAvatarResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при сохранении профиля в базе данных");
            }

            return Result<UpdateAvatarResponse>.Success(new UpdateAvatarResponse(
                AvatarUrl: $"/avatars/{userId}.png"
            ));
        }
        public async Task<Result<bool>> DeleteAvatar(EntityId<User> userId, string avatarPath, CancellationToken ct = default)
        {
            var user = await _users.GetByIdAsync(userId, [u => u.Profile], ct);
            if (user == null)
                return Result<bool>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            if (!Directory.Exists(avatarPath))
                Directory.CreateDirectory(avatarPath);
            var avatarFilePath = Path.Combine(avatarPath, $"{userId}.png");
            var profile = user.Profile;
            if (profile != null)
                profile.UpdateAvatar(null);
            if (File.Exists(avatarFilePath))
            {
                try
                {
                    File.Delete(avatarFilePath);
                }
                catch (Exception ex)
                {
                    _logger.Error("Ошибка при удалении файла аватара", ex);
                    return Result<bool>.Failed(ErrorCode.InternalServerError, "Ошибка при удалении файла аватара");
                }
            }
            return Result<bool>.Success(true);
        }
    }
}