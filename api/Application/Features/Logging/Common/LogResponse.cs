namespace Application.Features.Logging;

public record LogResponse(string Level, string Message, DateTime Timestamp, string? Exception = null);