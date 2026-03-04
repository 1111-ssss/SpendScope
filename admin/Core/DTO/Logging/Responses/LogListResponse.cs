namespace admin.Core.DTO.Logging.Responses;

public record LogListResponse(
    int TotalCount,
    int PageSize,
    int CurrentPage,
    int TotalPages,
    List<LogResponse> Items
);
