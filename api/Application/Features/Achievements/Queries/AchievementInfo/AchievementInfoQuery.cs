using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Achievements.AchievementInfo;

public record AchievementInfoQuery(int AchievementId) : IRequest<Result<AchievementResponse>>;