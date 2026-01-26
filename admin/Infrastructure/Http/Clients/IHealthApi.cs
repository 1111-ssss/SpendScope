using admin.Core.DTO.Health.Responses;
using Refit;

namespace admin.Infrastructure.Http.Clients;

public interface IHealthApi
{
    [Get("/health")]
    Task GetHealth(CancellationToken ct = default!);

    [Get("/health/ping")]
    Task<DateTime> GetPing(CancellationToken ct = default!);

    [Get("/health/requests")]
    Task<int> GetRequests([Query] DateTime dt, CancellationToken ct = default!);

    [Get("/health/detailed")]
    Task<HealthResponse> GetDetailed(CancellationToken ct = default!);
}