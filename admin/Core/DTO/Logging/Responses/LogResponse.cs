namespace admin.Core.DTO.Logging.Responses;
public record LogResponse(
    string Level,
    string Message,
    DateTime Timestamp,
    string? Exception = null
);
