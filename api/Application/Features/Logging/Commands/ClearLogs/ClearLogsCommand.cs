using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Logging.ClearLogs;

public record ClearLogsCommand(int OlderThanDays = -1) : IRequest<Result<ClearLogsResponse>>;