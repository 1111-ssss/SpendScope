using admin.Core.Interfaces;
using admin.Core.Model;
using admin.Core.DTO.Auth.Requests;
using admin.Core.DTO.Auth.Responses;
using Refit;
using System.Net;
using admin.Infrastructure.Http.Clients;
using System.Net.Http;

namespace admin.Infrastructure.Http.Auth;

public class TokenService : ITokenService
{
    private readonly IStorageService _storage;
    private readonly IAuthApi _authApi;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private TokenInfo? _cachedTokenInfo;

    public event EventHandler<TokenInfo?>? TokenInfoChanged;

    public TokenService(
        IStorageService storage,
        IAuthApi authApi
    )
    {
        _storage = storage;
        _authApi = authApi;
    }

    public async Task<string?> GetAccessTokenAsync(CancellationToken ct = default)
    {
        var tokenInfo = _cachedTokenInfo;
        if (tokenInfo == null || tokenInfo.ExpiresAt < DateTimeOffset.UtcNow.AddMinutes(1))
            tokenInfo = await _storage.GetTokenAsync(ct);

        if (tokenInfo == null) return null;

        if (tokenInfo.ExpiresAt > DateTimeOffset.UtcNow.AddMinutes(1))
            return tokenInfo.JwtToken;

        return await TryRefreshTokenAsync(ct);
    }
    private async Task<string?> TryRefreshTokenAsync(CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            var info = await _storage.GetTokenAsync(ct);
            if (info == null || string.IsNullOrEmpty(info.RefreshToken))
                return null;

            if (info.ExpiresAt > DateTimeOffset.UtcNow.AddSeconds(60))
                return info.JwtToken;

            try
            {
                var response = await _authApi.RefreshToken(
                    new RefreshTokenRequest(
                        JwtToken: info.JwtToken,
                        RefreshToken: info.RefreshToken
                    ),
                    ct
                );

                var newInfo = new TokenInfo(
                    JwtToken: response.JwtToken,
                    RefreshToken: response.RefreshToken,
                    ExpiresAt: response.ExpiresAt
                );

                await _storage.SaveTokenAsync(newInfo, ct);
                _cachedTokenInfo = newInfo;
                return newInfo.JwtToken;
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest || ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _storage.ClearTokenAsync(ct);
                _cachedTokenInfo = null;
                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    public async Task SaveTokenAsync(AuthResponse response, CancellationToken ct = default)
    {
        var info = new TokenInfo(
            JwtToken: response.JwtToken,
            RefreshToken: response.RefreshToken,
            ExpiresAt: response.ExpiresAt
        );

        await SaveTokenAsync(info, ct);
    }
    public async Task SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct = default)
    {
        TokenInfoChanged?.Invoke(this, tokenInfo);
        _cachedTokenInfo = tokenInfo;
        await _storage.SaveTokenAsync(tokenInfo, ct);
    }

    public async Task ClearAsync(CancellationToken ct = default)
    {
        TokenInfoChanged?.Invoke(this, null);
        _cachedTokenInfo = null;
        await _storage.ClearTokenAsync(ct);
    }
}