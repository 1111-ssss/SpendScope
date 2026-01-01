using Application.Abstractions.Interfaces;
using Application.DTO.Achievement;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Logger;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Application.Service.Achievements.Handlers
{
    public class AchievementIconHandle
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Achievement> _ach;
        private readonly ICustomLogger<AddAchievementHandle> _logger;
        public AchievementIconHandle(ICustomLogger<AddAchievementHandle> logger, IUnitOfWork db, IRepository<Achievement> ach)
        {
            _logger = logger;
            _db = db;
            _ach = ach;
        }
        public async Task<Result<GetAchievementResponse>> Handle(EntityId<Achievement> achId, IFormFile file, string achPath, CancellationToken ct = default)
        {
            var ach = await _ach.GetByIdAsync((int)achId, ct);
            
            if (ach == null)
                return Result<GetAchievementResponse>.Failed(ErrorCode.BadRequest, "Достижение не найдено");

            if (file == null || file.Length == 0)
                return Result<GetAchievementResponse>.Failed(ErrorCode.BadRequest, "Файл не загружен");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                return Result<GetAchievementResponse>.Failed(ErrorCode.BadRequest, "Файл должен иметь расширение .png, .jpg или .jpeg");

            if (!Directory.Exists(achPath))
                Directory.CreateDirectory(achPath);

            var iconPath = Path.Combine(achPath, $"{achId}.png");
            try
            {
                using var image = await Image.LoadAsync(file.OpenReadStream(), ct);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(512, 512),
                    Mode = ResizeMode.Max
                }));

                await image.SaveAsPngAsync(iconPath, ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при обработке изображения аватара", ex);
                return Result<GetAchievementResponse>.Failed(ErrorCode.BadRequest, "Невалидное или повреждённое изображение");
            }
            
            ach.UpdateIcon($"achievements/{achId}.png");
            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при изменения иконки достижения в бд", ex);
                return Result<GetAchievementResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при изменения иконки достижения в бд");
            }

            return Result<GetAchievementResponse>.Success(new GetAchievementResponse(
                Name: ach.Name,
                Description: ach.Description ?? "",
                IconUrl: ach.IconUrl ?? "achievements/default-icon.png"
            ));
        }
    }
}