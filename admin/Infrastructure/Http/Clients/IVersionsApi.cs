using admin.Core.DTO.Versions.Responses;
using Refit;

namespace admin.Infrastructure.Http.Clients;

public interface IVersionsApi
{
    [Get("/versions/{branch}")]
    Task<AppVersionResponse> GetLatest([Query] string branch, CancellationToken ct = default);

    [Get("/versions")]
    Task<AllVersionsResponse> GetAllVersions(CancellationToken ct = default);

    [Delete("/versions/{branch}/{build}")]
    Task DeleteVersion([Query] string branch, [Query] string build, CancellationToken ct = default);

    [Multipart]
    [Post("/versions/upload")]
    Task<AppVersionResponse> UploadVersion(string branch, int build, string? changelog, StreamPart file);
}
