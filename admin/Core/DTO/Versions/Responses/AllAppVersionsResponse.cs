namespace admin.Core.DTO.Versions.Responses;

public record AllVersionsResponse(
    Dictionary<string, List<AppVersionResponse>> Versions
);