using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Achievements.AddAchievement;

public class AddAchievementCommandHandler : IRequestHandler<AddAchievementCommand, Result<AchievementResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Achievement> _achievementRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IImageFormatter _imageFormatter;
    private readonly ILogger<AddAchievementCommandHandler> _logger;
    public AddAchievementCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<Achievement> achievementRepository,
        IFileStorage fileStorage,
        IImageFormatter imageFormatter,
        ILogger<AddAchievementCommandHandler> logger)
    {
        _uow = uow;
        _achievementRepository = achievementRepository;
        _fileStorage = fileStorage;
        _imageFormatter = imageFormatter;
        _logger = logger;
    }

    public async Task<Result<AchievementResponse>> Handle(AddAchievementCommand request, CancellationToken ct)
    {
        var ach = Achievement.Create(
            name: request.Name,
            description: request.Description,
            iconUrl: null
        );

        await _achievementRepository.AddAsync(ach, ct);

        var iconName = $"{Guid.NewGuid()}.png";
        ach.Update(iconUrl: $"achievements/{iconName}");

        try
        {
            await _uow.SaveChangesAsync(ct);

            var result = await _imageFormatter.FormatImage(request.Image, $"achievements/{iconName}", ct);

            return result.Bind(() => new AchievementResponse(
                Name: ach.Name,
                Description: ach.Description ?? "",
                IconUrl: ach.IconUrl ?? "achievements/default-icon.png"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при сохранении достижения");
            return Result<AchievementResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при сохранении достижения");
        }
    }
}