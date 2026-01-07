using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Application.Common.Responses;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Achievements.AchievementIcon;

public class AchievementIconQueryHandler : IRequestHandler<AchievementIconQuery, Result<FileDownloadResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Achievement> _achievementRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<AchievementIconQueryHandler> _logger;
    public AchievementIconQueryHandler(
        IUnitOfWork uow,
        IBaseRepository<Achievement> achievementRepository,
        IFileStorage fileStorage,
        ILogger<AchievementIconQueryHandler> logger)
    {
        _uow = uow;
        _achievementRepository = achievementRepository;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<Result<FileDownloadResponse>> Handle(AchievementIconQuery request, CancellationToken ct)
    {
        var ach = await _achievementRepository.GetByIdAsync((EntityId<Achievement>)request.AchievementId, ct);

        if (ach == null)
            return Result<FileDownloadResponse>.Failed(ErrorCode.NotFound, "Достижение не найдено");

        var iconPath = _fileStorage.GetFilePath(ach.IconUrl, "achievements/default-icon.png");

        if (iconPath == null)
            return Result<FileDownloadResponse>.Failed(ErrorCode.NotFound, "Иконка достижения не найдена");

        return Result<FileDownloadResponse>.Success(new FileDownloadResponse(
            FilePath: iconPath,
            ContentType: "image/png"
        ));
    }
}