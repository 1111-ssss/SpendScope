using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTO.Profile
{
    public record GetProfileResponse(string? DisplayName, string? AvatarUrl, string? Bio, DateTime? LastOnline);
}