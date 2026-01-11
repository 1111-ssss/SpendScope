using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.GetLogs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Logging.ClearLogs;

public class ClearLogsCommandHandler : IRequestHandler<ClearLogsCommand, Result<ClearLogsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<LogEntry> _logsRepository;
    private readonly ILogger<ClearLogsCommandHandler> _logger;
    public ClearLogsCommandHandler(IUnitOfWork unitOfWork, IBaseRepository<LogEntry> logsRepository, ILogger<ClearLogsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logsRepository = logsRepository;
        _logger = logger;
    }
    public async Task<Result<ClearLogsResponse>> Handle(ClearLogsCommand request, CancellationToken ct)
    {
        var deletedCount = await _logsRepository.DeleteRangeAsync(new LogsByOlderThanSpec(request.OlderThanDays), ct);

        try
        {
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при очистке логов");
            return Result.InternalServerError("Ошибка при очистке логов");
        }

        return Result<ClearLogsResponse>.Success(new ClearLogsResponse(deletedCount));
    }
}