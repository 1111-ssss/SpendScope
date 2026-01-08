using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Logging.GetLogs;

public record GetLogsQuery(
    int? Level = null,
    int MinimalLevel = 2,
    int Page = 1,
    int PageSize = 50,
    string OrderBy = "Timestamp",
    bool IsDesc = true,
    string? Search = null
) : IRequest<Result<LogListResponse>>;