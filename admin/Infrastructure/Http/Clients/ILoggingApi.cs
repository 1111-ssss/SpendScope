using admin.Core.DTO.Logging.Responses;
using Refit;

namespace admin.Infrastructure.Http.Clients;

public interface ILoggingApi
{
    [Get("/logging")]
    Task<LogListResponse> Get(
        [Query] int? level = null,
        [Query] int minimalLevel = 2,
        [Query] int page = 1,
        [Query] int pageSize = 50,
        [Query] string orderBy = "Timestamp",
        [Query] bool isDesc = true,
        [Query] string? search = null
    );

    [Delete("/logging")]
    Task<ClearLogsResponse> ClearLogs(
        [Query] int olderThanDays = -1
    );
}