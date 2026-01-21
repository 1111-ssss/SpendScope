using admin.Core.Interfaces;
using admin.Core.Model;
using admin.Features.Auth.DTO.Requests;
using admin.Features.Auth.DTO.Responses;
using Refit;
using System.Net;

namespace admin.Infrastructure.Http.Auth;

public class TokenService : ITokenService
{
    private readonly IStorageService _storage;
    private readonly IApiService _apiService;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public TokenService(
        IStorageService storage,
        IApiService apiService
    )
    {
        _storage = storage;
        _apiService = apiService;
    }

    public async Task<string?> GetAccessTokenAsync(CancellationToken ct = default)
    {
        var info = await _storage.GetTokenAsync(ct);
        if (info == null) return null;

        if (info.ExpiresAt > DateTimeOffset.UtcNow.AddMinutes(1))
            return info.JwtToken;

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
                var response = await _apiService.Auth.RefreshToken(
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
                return newInfo.JwtToken;
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest || ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _storage.ClearTokenAsync(ct);
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

        await _storage.SaveTokenAsync(info, ct);
    }
    public async Task SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct = default) =>
        await _storage.SaveTokenAsync(tokenInfo, ct);

    public Task ClearAsync(CancellationToken ct = default) => _storage.ClearTokenAsync(ct);
}