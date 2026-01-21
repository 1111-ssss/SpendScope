using admin.Core.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using admin.Core.Model;
using admin.Features.Auth.DTO.Responses;

namespace admin.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ITokenService _tokenService;
    //private readonly IWindowNavigationController _windowNavigation;
    private ClaimsPrincipal? _currentPrincipal;

    /*TODO:
     * сделать ивент для изменения jwt
    */

    public CurrentUserService(
        ITokenService tokenService
        //IWindowNavigationController windowNavigationController
    )
    {
        _tokenService = tokenService;
        //_windowNavigation = windowNavigationController;

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
    private void SetFromToken(string token)
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
    public async Task LoginAsync(TokenInfo tokenInfo)
    {
        await _tokenService.SaveTokenAsync(tokenInfo);
        SetFromToken(tokenInfo.JwtToken);
    }
    public async Task LoginAsync(AuthResponse response)
    {
        var tokenInfo = new TokenInfo(
            JwtToken: response.JwtToken,
            RefreshToken: response.RefreshToken,
            ExpiresAt: response.ExpiresAt
        );
        await LoginAsync(tokenInfo);
    }
    public async Task LogoutAsync()
    {
        await _tokenService.ClearAsync();
        _currentPrincipal = null;
    }

    public bool IsAuthenticated => _currentPrincipal?.Identity?.IsAuthenticated ?? false;
    public bool IsAdmin => _currentPrincipal?.IsInRole("Admin") ?? false;
    public string? UserId => _currentPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _currentPrincipal?.FindFirst(ClaimTypes.Name)?.Value;
}
