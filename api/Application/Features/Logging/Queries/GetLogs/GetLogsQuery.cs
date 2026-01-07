using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Logging.GetLogs;

public record GetLogsQuery(string? Level = null, string MinimalLevel = "Information", int Skip = 0, int Take = 50) : IRequest<Result<List<LogResponse>>>;