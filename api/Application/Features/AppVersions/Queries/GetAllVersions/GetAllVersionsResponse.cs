namespace Application.Features.AppVersions.GetAllVersions;

public record GetAllVersionsResponse(
    Dictionary<string, List<AppVersionResponse>> Versions
);