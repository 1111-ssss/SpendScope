namespace Application.Features.Logging;

public record LogResponse(string Level, string Message, string? Exception = null);