using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.GetLogs;

public class GetLogsSpec : Specification<LogEntry>
{
    public GetLogsSpec(
        int? level = null,
        int minimalLevel = 2, //Information
        int page = 1,
        int pageSize = 50,
        string orderBy = "Timestamp",
        bool isDesc = true,
        string? search = null
    )
    {
        if (level.HasValue)
        {
            Query.Where(l => l.Level == level);
        }

        Query.Where(l => l.Level >= minimalLevel);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            Query.Where(l => l.Message.Contains(search) ||
                (l.Exception != null && l.Exception.Contains(search)));
        }

        var orderByLower = orderBy?.ToLowerInvariant();
        if (orderByLower == "level")
        {
            if (isDesc)
                Query.OrderByDescending(l => l.Level);
            else
                Query.OrderBy(l => l.Level);
        }
        else
        {
            if (isDesc)
                Query.OrderByDescending(l => l.Timestamp);
            else
                Query.OrderBy(l => l.Timestamp);
        }

        Query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}