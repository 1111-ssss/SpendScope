using admin.Core.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using admin.Core.Model;
using admin.Core.DTO.Auth.Responses;

namespace admin.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ITokenService _tokenService;
    private ClaimsPrincipal? _currentPrincipal;

    public event EventHandler? UserStateChanged;

    public CurrentUserService(
        ITokenService tokenService
    )
    {
        _tokenService = tokenService;

        _ = InitializeAsync();
        tokenService.TokenInfoChanged += OnTokenInfoChanged;
    }

    private void OnTokenInfoChanged(object? sender, TokenInfo? tokenInfo)
    {
        SetFromToken(tokenInfo?.JwtToken);
    }
    private async Task InitializeAsync()
    {
        var token = await _tokenService.GetAccessTokenAsync();

        SetFromToken(token);
    }
    private void SetFromToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            UserStateChanged?.Invoke(this, EventArgs.Empty);
            return;
        }
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
        finally
        {
            UserStateChanged?.Invoke(this, EventArgs.Empty);
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
        await _tokenService.ClearAsync(); //сам и вызовет UserStateChanged
        _currentPrincipal = null;
        //UserStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsAuthenticated => _currentPrincipal?.Identity?.IsAuthenticated ?? false;
    public bool IsAdmin => _currentPrincipal?.IsInRole("Admin") ?? false;
    public string? UserId => _currentPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _currentPrincipal?.FindFirst(ClaimTypes.Name)?.Value;
}
