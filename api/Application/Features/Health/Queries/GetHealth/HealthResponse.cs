using Domain.Abstractions.Enums;

namespace Application.Features.Health.GetHealth;

public record HealthResponse(
    bool IsHealthy,
    UnhealthyReason UnhealthyReasons,
    bool IsDBHealthy,
    TimeSpan Uptime,
    double CpuUsage,
    long MemoryUsage,
    long DiskUsage,
    long ActiveConnections,
    int FailedRequests,
    int TotalRequests,
    long DbLatency,
    DateTime CurrentTime
);