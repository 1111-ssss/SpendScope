using Application.Abstractions.Interfaces;
using Application.DTO.Achievement;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;

namespace Application.Service.Achievements.Handlers
{
    public class GetAchievementHandle
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Achievement> _ach;
        public GetAchievementHandle(IUnitOfWork db, IRepository<Achievement> ach)
        {
            _db = db;
            _ach = ach;
        }
        public async Task<Result<GetAchievementResponse>> Handle(EntityId<Achievement> achId, CancellationToken ct = default)
        {
            var ach = await _ach.GetByIdAsync(achId, ct);

            if (ach == null)
                return Result<GetAchievementResponse>.Failed(ErrorCode.NotFound, "Достижение не найдено");

            return Result<GetAchievementResponse>.Success(new GetAchievementResponse(
                Name: ach.Name,
                Description: ach.Description ?? "",
                IconUrl: ach.IconUrl ?? "achievements/default-icon.png"
            ));
        }
    }
}