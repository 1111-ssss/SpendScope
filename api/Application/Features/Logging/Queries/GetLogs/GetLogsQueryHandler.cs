using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.GetLogs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Logging.GetLogs;

public class GetLogsQueryHandler : IRequestHandler<GetLogsQuery, Result<LogListResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<LogEntry> _logsRepository;
    private readonly ILogger<GetLogsQueryHandler> _logger;
    public GetLogsQueryHandler(IUnitOfWork unitOfWork, IBaseRepository<LogEntry> logsRepository, ILogger<GetLogsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logsRepository = logsRepository;
        _logger = logger;
    }
    public async Task<Result<LogListResponse>> Handle(GetLogsQuery request, CancellationToken ct)
    {
        var logs = await _logsRepository.ListAsync(
            new GetLogsSpec(
                request.Level,
                request.MinimalLevel,
                request.Page,
                request.PageSize,
                request.OrderBy,
                request.IsDesc,
                request.Search
            ), ct);
        
        var totalCount = await _logsRepository.CountAsync(
            new GetLogsSpec(
                request.Level,
                request.MinimalLevel,
                1,
                int.MaxValue,
                request.OrderBy,
                request.IsDesc,
                request.Search
            ), ct);

        var logsResponse = logs.Select(
            l => new LogResponse(Enum.GetName(typeof(LogLevel), l.Level) ?? "Unknown", l.Message, l.Timestamp ?? DateTime.UtcNow, l.Exception)
        ).ToList();

        return Result<LogListResponse>.Success(new LogListResponse(
            TotalCount: totalCount,
            PageSize: request.PageSize,
            CurrentPage: request.Page,
            TotalPages: (int)Math.Ceiling((double)totalCount / request.PageSize),
            Items: logsResponse
        ));
    }
}