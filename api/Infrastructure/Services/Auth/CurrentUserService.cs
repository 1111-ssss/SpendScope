using System.Security.Claims;
using Application.Abstractions.Auth;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public int? GetUserId() => int.TryParse(
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        out var id
    ) ? id : null;
    public string GetUserIp() => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "";
    public bool IsAdmin() => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value == "Admin";
}