using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Web.Extensions.DependencyInjection;

public static class RateLimitExtension
{
    public static IServiceCollection AddRateLimit(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddTokenBucketLimiter("DefaultLimiter", opt =>
            {
                opt.TokenLimit = 50;
                opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                opt.TokensPerPeriod = 25;
                opt.AutoReplenishment = true;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 5;
            });

            options.AddFixedWindowLimiter("StrictLimiter", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromSeconds(10);
            });

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var key = context.User.Identity?.IsAuthenticated == true
                    ? $"user:{context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value}"
                    : $"ip:{context.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

                return RateLimitPartition.GetTokenBucketLimiter(key, _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 150,
                    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                    TokensPerPeriod = 60,
                    AutoReplenishment = true,
                    QueueLimit = 10,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers.RetryAfter = "60";
                await context.HttpContext.Response.WriteAsync("Слишком много запросов. Подождите и попробуйте снова.", token);
            };
        });
        return services;
    }
}