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

            if (context.Response.StatusCode >= 400)
                reqStatsService.AddFailedRequest();
        }
        catch
        {
            reqStatsService.AddFailedRequest();
            throw;
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