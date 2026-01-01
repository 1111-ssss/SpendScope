namespace Application.Features.AppVersions
{
    public record AppVersionResponse(string Branch, int Build, string? Changelog, DateTime? UploadedAt, string UploadedBy);
}