using admin.Core.Model;
using admin.Core.DTO.Auth.Responses;

namespace admin.Core.Interfaces;

public interface ITokenService
{
    public Task<string?> GetAccessTokenAsync(CancellationToken ct = default);
    public Task SaveTokenAsync(AuthResponse response, CancellationToken ct = default);
    public Task SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct = default);
    public Task ClearAsync(CancellationToken ct = default);
    public event EventHandler<TokenInfo?> TokenInfoChanged;
}
