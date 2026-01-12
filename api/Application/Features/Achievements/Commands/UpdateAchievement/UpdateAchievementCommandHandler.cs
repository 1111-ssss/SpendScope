using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Achievements.UpdateAchievement;

public class UpdateAchievementCommandHandler : IRequestHandler<UpdateAchievementCommand, Result<AchievementResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Achievement> _achievementRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IImageFormatter _imageFormatter;
    private readonly ILogger<UpdateAchievementCommandHandler> _logger;
    public UpdateAchievementCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<Achievement> achievementRepository,
        IFileStorage fileStorage,
        IImageFormatter imageFormatter,
        ILogger<UpdateAchievementCommandHandler> logger)
    {
        _uow = uow;
        _achievementRepository = achievementRepository;
        _fileStorage = fileStorage;
        _imageFormatter = imageFormatter;
        _logger = logger;
    }

    public async Task<Result<AchievementResponse>> Handle(UpdateAchievementCommand request, CancellationToken ct)
    {
        var ach = await _achievementRepository.GetByIdAsync((EntityId<Achievement>)request.AchievementId, ct);

        if (ach == null)
            return Result.NotFound("Достижение не найдено");

        var iconPath = _fileStorage.GetFilePath(ach.IconUrl);
        if (iconPath == null && request.Image != null)
        {
            ach.Update(iconUrl: $"achievements/{Guid.NewGuid()}.png");
        }

        ach.Update(name: request.Name, description: request.Description);

        await _achievementRepository.UpdateAsync(ach, ct);

        try
        {
            await _uow.SaveChangesAsync(ct);

            var response = new AchievementResponse(
                Name: ach.Name,
                Description: ach.Description ?? "",
                IconUrl: ach.IconUrl ?? "achievements/default-icon.png"
            );

            if (request.Image != null)
            {
                var result = await _imageFormatter.FormatImageAsync(request.Image, ach.IconUrl!, ct);
                return result.Bind(() => response);
            }
            return Result<AchievementResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении достижения");
            return Result.InternalServerError("Ошибка при обновлении достижения");
        }
    }
}