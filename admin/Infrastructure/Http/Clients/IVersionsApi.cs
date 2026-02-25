using admin.Core.DTO.Versions.Responses;
using Refit;

namespace admin.Infrastructure.Http.Clients;

public interface IVersionsApi
{
    [Get("/versions/getlatest")]
    Task<AppVersionResponse> GetLatest([Query] string version);
}
