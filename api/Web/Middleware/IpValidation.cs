using System.Net;
using Application.Abstractions.Auth;
using System.Net.Sockets;

namespace Web.Middleware;

public class IpValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IpValidationMiddleware> _logger;
    private static readonly string[] PublicPaths =
        [
            "/auth",
            "/swagger"
        ];
    public IpValidationMiddleware(RequestDelegate next, ILogger<IpValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            await _next(context);
            return;
        }

        if (IsPublicPath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var currentip = currentUserService.GetUserIp();

        if (string.IsNullOrWhiteSpace(currentip))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Айпи не определен");
            return;
        }

        var ipClaim = context.User.FindFirst("ip")?.Value;

        if (string.IsNullOrWhiteSpace(ipClaim))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Айпи не указан");
            return;
        }

        if (!AreIpAddressesEqual(ipClaim, currentip))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            _logger.LogWarning($"Попытка доступа с некорректного айпи: токен айпи {ipClaim}, текущий айпи {currentip}");
            await context.Response.WriteAsync("Айпи не совпадает");
            return;
        }

        // _logger.LogInformation($"Айпи: {currentip} == {ipClaim}");

        await _next(context);
    }
    private static bool IsPublicPath(PathString path)
    {
        if (PublicPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }
        return false;
    }
    private static bool AreIpAddressesEqual(string? ip1, string? ip2)
    {
        if (!IPAddress.TryParse(ip1, out var addr1) ||
            !IPAddress.TryParse(ip2, out var addr2))
            return false;

        if (addr1.IsIPv4MappedToIPv6 && addr2.AddressFamily == AddressFamily.InterNetwork)
            addr1 = addr1.MapToIPv4();
        else if (addr2.IsIPv4MappedToIPv6 && addr1.AddressFamily == AddressFamily.InterNetwork)
            addr2 = addr2.MapToIPv4();

        return addr1.Equals(addr2);
    }
}

public static class IpValidationExtensions
{
    public static IApplicationBuilder UseIPValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IpValidationMiddleware>();
    }
}