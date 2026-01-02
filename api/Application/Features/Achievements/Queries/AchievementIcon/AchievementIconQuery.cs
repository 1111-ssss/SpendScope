using Application.Common.Responses;
using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Achievements.AchievementIcon
{
    public record AchievementIconQuery(int AchievementId) : IRequest<Result<FileDownloadResponse>>;
}