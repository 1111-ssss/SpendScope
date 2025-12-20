namespace Application.DTO.AppVersion
{
    public record UploadVersionResponse(
        int Build,
        string DownloadUrl,
        string? Changelog,
        DateTime? UploadedAt,
        string UploadedBy
    );
}