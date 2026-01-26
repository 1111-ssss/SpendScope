using Application.Abstractions.Misc;

namespace Web.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, IRequestStatisticsService reqStatsService)
    {
        try {
            reqStatsService.AddRequest(DateTime.UtcNow);
            reqStatsService.EnterRequest();

            await _next(context);
        }
        catch
        {
            reqStatsService.AddFailedRequest();
        }
        finally {
            reqStatsService.ExitRequest();
        }
    }
}

public static class RequestLoggingExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}