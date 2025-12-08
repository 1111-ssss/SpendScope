namespace Application.DTO.AppVersion
{
    public record GetLatestVersionResponse(
        int Build,
        string DownloadUrl,
        string? Changelog,
        DateTime? UploadedAt,
        string UploadedBy
    );
}