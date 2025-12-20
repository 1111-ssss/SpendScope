using Domain.Entities;
using Domain.ValueObjects;

namespace Application.DTO.AppVersion
{
    public record UploadVersionRequest(string Branch, string Build, EntityId<User> UploadedBy, string? Changelog);
}