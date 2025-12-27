using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTO.Achievement
{
    public record AddAchievementRequest(string Name, string? Description);
}