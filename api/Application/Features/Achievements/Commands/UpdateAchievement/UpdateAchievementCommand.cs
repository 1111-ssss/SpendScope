using Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Achievements.UpdateAchievement
{
    public record UpdateAchievementCommand(int AchievementId, string? Name, string? Description, IFormFile? Image) : IRequest<Result<AchievementResponse>>;
}