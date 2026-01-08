using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.GetLogs;

public class LogsByOlderThanSpec : Specification<LogEntry>
{
    public LogsByOlderThanSpec(int olderThanDays)
    {
        Query.Where(l => l.Timestamp <= DateTime.UtcNow.AddDays(-olderThanDays));
    }
}