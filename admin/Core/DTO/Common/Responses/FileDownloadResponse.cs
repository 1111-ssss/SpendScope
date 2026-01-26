namespace admin.Core.DTO.Common.Responses;

public record FileDownloadResponse(
    string FilePath,
    string ContentType,
    string? FileName = null
);