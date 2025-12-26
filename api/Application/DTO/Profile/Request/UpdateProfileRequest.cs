using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTO.Profile
{
    public record UpdateProfileRequest(string? DisplayName, string? Bio);
}