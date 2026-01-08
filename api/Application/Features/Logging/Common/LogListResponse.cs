namespace Application.Features.Logging;

public record LogListResponse(int TotalCount, int PageSize, int CurrentPage, int TotalPages, List<LogResponse> Items);