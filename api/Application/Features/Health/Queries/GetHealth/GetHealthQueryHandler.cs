using System.Diagnostics;
using Application.Abstractions.DataBase;
using Application.Abstractions.Misc;
using Application.Abstractions.Storage;
using Domain.Abstractions.Enums;
using Domain.Abstractions.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Health.GetHealth;

public class GetHealthQueryHandler : IRequestHandler<GetHealthQuery, Result<HealthResponse>>
{
    private static readonly DateTime _startupTime = DateTime.UtcNow;
    private static readonly Process _currentProcess = Process.GetCurrentProcess();
    private readonly IUnitOfWork _uow;
    private readonly IFileStorage _fileStorage;
    private readonly ICpuUsageService _cpuUsageService;
    private readonly IRequestStatisticsService _reqStatsService;
    private readonly ILogger<GetHealthQueryHandler> _logger;
    public GetHealthQueryHandler(
        IUnitOfWork uow,
        IFileStorage fileStorage,
        ICpuUsageService cpuUsageService,
        IRequestStatisticsService requestStatisticsService,
        ILogger<GetHealthQueryHandler> logger)
    {
        _uow = uow;
        _fileStorage = fileStorage;
        _cpuUsageService = cpuUsageService;
        _reqStatsService = requestStatisticsService;
        _logger = logger;
    }
    public async Task<Result<HealthResponse>> Handle(GetHealthQuery request, CancellationToken ct)
    {
        var dbHealthy = await _uow.CanConnectAsync(ct);
        // var memoryUsageMb = GC.GetTotalMemory(false) / 1024 / 1024;
        var memoryUsageMb = _currentProcess.WorkingSet64 / 1024 / 1024;
        var cpuUsagePerc = _cpuUsageService.GetCpuUsagePercent();
        var diskUsageMb = _fileStorage.GetStorageSize(null) / 1024 / 1024;
        var uptime = DateTime.UtcNow - _startupTime;
        var dbLatency = await _uow.CalcDBLatencyAsync(ct);
        var activeConnections = _reqStatsService.GetActiveConnections();
        var failedRequests = _reqStatsService.GetFailedRequests();
        var totalRequests = _reqStatsService.GetTotalRequests();

        var unhealthyReasons = UnhealthyReason.None;
        if (!dbHealthy)
            unhealthyReasons |= UnhealthyReason.Database;
        if (cpuUsagePerc > 80)
            unhealthyReasons |= UnhealthyReason.HighCpu;
        if (memoryUsageMb > 10*1024)
            unhealthyReasons |= UnhealthyReason.HighMemory;
        if (diskUsageMb > 50*1024)
            unhealthyReasons |= UnhealthyReason.HighDisk;
        if (dbLatency < 0)
            unhealthyReasons |= UnhealthyReason.DbLatency;

        return Result<HealthResponse>.Success(new HealthResponse(
            IsHealthy: unhealthyReasons == UnhealthyReason.None,
            UnhealthyReasons: unhealthyReasons,
            IsDBHealthy: dbHealthy,
            CpuUsage: cpuUsagePerc,
            MemoryUsage: memoryUsageMb,
            Uptime: uptime,
            DiskUsage: diskUsageMb,
            ActiveConnections: activeConnections,
            FailedRequests: failedRequests,
            TotalRequests: totalRequests,
            DbLatency: dbLatency
        ));
    }
}