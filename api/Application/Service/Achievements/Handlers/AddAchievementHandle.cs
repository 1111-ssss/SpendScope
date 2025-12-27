using Application.Abstractions.Interfaces;
using Application.DTO.Achievement;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;
using Logger;

namespace Application.Service.Achievements.Handlers
{
    public class AddAchievementHandle
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Achievement> _ach;
        private readonly ICustomLogger<AddAchievementHandle> _logger;
        public AddAchievementHandle(ICustomLogger<AddAchievementHandle> logger, IUnitOfWork db, IRepository<Achievement> ach)
        {
            _logger = logger;
            _db = db;
            _ach = ach;
        }
        public async Task<Result<GetAchievementResponse>> Handle(AddAchievementRequest achData, CancellationToken ct = default)
        {
            var ach = Achievement.Create(achData.Name, description: achData.Description ?? "Достижение");

            await _ach.AddAsync(ach, ct);
            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при записи достижения в бд", ex);
                return Result<GetAchievementResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при записи достижения в бд");
            }

            return Result<GetAchievementResponse>.Success(new GetAchievementResponse(
                Name: ach.Name,
                Description: ach.Description ?? "",
                IconUrl: ach.IconUrl ?? "achievements/default-icon.png"
            ));
        }
    }
}