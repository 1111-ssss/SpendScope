using admin.Core.Enums;

namespace admin.Core.DTO.Health.Responses;

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