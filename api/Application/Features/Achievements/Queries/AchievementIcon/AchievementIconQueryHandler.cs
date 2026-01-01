using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Achievements.AchievementIcon
{

    public class AchievementIconQueryHandler : IRequestHandler<AchievementIconQuery, Result<AchievementIconResponse>>
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

        public async Task<Result<AchievementIconResponse>> Handle(AchievementIconQuery request, CancellationToken ct)
        {
            var ach = await _achievementRepository.GetByIdAsync(request.AchievementId, ct);

            if (ach == null)
                return Result<AchievementIconResponse>.Failed(ErrorCode.NotFound, "Достижение не найдено");

            var iconPath = _fileStorage.GetFilePath(ach.IconUrl ?? "");

            if (iconPath == null)
                return Result<AchievementIconResponse>.Failed(ErrorCode.NotFound, "Иконка достижения не найдена");

            return Result<AchievementIconResponse>.Success(new AchievementIconResponse(
                FilePath: iconPath
            ));
        }
    }
}