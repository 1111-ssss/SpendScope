using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Achievements.AchievementInfo
{
    public class AchievementInfoQueryHandler : IRequestHandler<AchievementInfoQuery, Result<AchievementResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Achievement> _achievementRepository;
        private readonly ILogger<AchievementInfoQueryHandler> _logger;
        public AchievementInfoQueryHandler(
            IUnitOfWork uow,
            IBaseRepository<Achievement> achievementRepository,
            ILogger<AchievementInfoQueryHandler> logger)
        {
            _uow = uow;
            _achievementRepository = achievementRepository;
            _logger = logger;
        }

        public async Task<Result<AchievementResponse>> Handle(AchievementInfoQuery request, CancellationToken ct)
        {
            var ach = await _achievementRepository.GetByIdAsync(request.AchievementId, ct);

            if (ach == null)
                return Result<AchievementResponse>.Failed(ErrorCode.NotFound, "Достижение не найдено");

            return Result<AchievementResponse>.Success(new AchievementResponse(
                Name: ach.Name,
                Description: ach.Description ?? "",
                IconUrl: ach.IconUrl ?? ""
            ));
        }
    }
}