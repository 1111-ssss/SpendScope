using System.Collections.Concurrent;
using Application.Abstractions.Misc;

namespace Infrastructure.Services.Misc;

public class RequestStatisticsService : IRequestStatisticsService
{
    private readonly ConcurrentDictionary<DateTime, int> _requestCount = new();
    public void AddRequest(DateTime dateTime)
    {
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