using admin.Core.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace admin.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ITokenService _tokenService;
    private ClaimsPrincipal? _currentPrincipal;

    //потом мб ивент и тд и тп

    public CurrentUserService(ITokenService tokenService)
    {
        _tokenService = tokenService;

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var token = await _tokenService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            SetFromToken(token);
        }
    }
    public void SetFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var claims = jwt.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            _currentPrincipal = new ClaimsPrincipal(identity);
        }
        catch
        {
            _currentPrincipal = null;
        }
    }

    public bool IsAuthenticated => _currentPrincipal?.Identity?.IsAuthenticated ?? false;
    public bool IsAdmin => _currentPrincipal?.IsInRole("Admin") ?? false;
    public string? UserId => _currentPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _currentPrincipal?.FindFirst(ClaimTypes.Name)?.Value;
}
