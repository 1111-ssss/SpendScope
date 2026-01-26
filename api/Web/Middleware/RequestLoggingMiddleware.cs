using Application.Abstractions.Misc;

namespace Web.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRequestStatisticsService _reqStatsService;
    public RequestLoggingMiddleware(RequestDelegate next, IRequestStatisticsService reqStatsService)
    {
        _next = next;
        _reqStatsService = reqStatsService;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        _reqStatsService.AddRequest(DateTime.UtcNow);

        await _next(context);
    }
}

public static class RequestLoggingExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}