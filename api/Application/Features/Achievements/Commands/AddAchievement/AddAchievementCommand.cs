using Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Achievements.AddAchievement;

public record AddAchievementCommand(string Name, string Description, IFormFile Image) : IRequest<Result<AchievementResponse>>;