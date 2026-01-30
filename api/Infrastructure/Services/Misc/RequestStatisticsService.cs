using System.Collections.Concurrent;
using Application.Abstractions.Misc;

namespace Infrastructure.Services.Misc;

public class RequestStatisticsService : IRequestStatisticsService
{
    private readonly ConcurrentDictionary<DateTime, int> _requestCount = new();
    private int _activeConnections = 0;
    private int _failedRequests;
    private int _totalRequests;
    public void EnterRequest() => _activeConnections++;
    public void ExitRequest() => _activeConnections--;
    public int GetActiveConnections() => _activeConnections;
    public void AddFailedRequest() => _failedRequests++;
    public int GetFailedRequests() => _failedRequests;
    public int GetTotalRequests() => _totalRequests;
    public void AddRequest(DateTime dateTime)
    {
        _totalRequests++;
        _requestCount.AddOrUpdate(
            RoundToHour(dateTime),
            1,
            (_, count) => count + 1
        );

        var cutoffDt = DateTime.UtcNow.AddHours(-24);
        foreach (var key in _requestCount.Keys.Where(k => k < cutoffDt).ToArray())
        {
            _requestCount.TryRemove(key, out _);
        }
    }
    public int GetRequestCountAsync(DateTime dateTime)
    {
        var roundedDateTime = RoundToHour(dateTime);
        if (_requestCount.TryGetValue(roundedDateTime, out var count))
        {
            return count;
        }
        return 0;
    }
    private static DateTime RoundToHour(DateTime dt) => new(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, DateTimeKind.Utc);
}