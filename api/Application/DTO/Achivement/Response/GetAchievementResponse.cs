using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTO.Achievement
{
    public record GetAchievementResponse(string Name, string Description, string IconUrl);
}