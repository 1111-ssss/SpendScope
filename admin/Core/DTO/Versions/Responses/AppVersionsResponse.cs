namespace admin.Core.DTO.Versions.Responses;

public record AppVersionResponse(
    string Branch,
    int Build,
    string? Changelog,
    DateTime? UploadedAt,
    string UploadedBy
);