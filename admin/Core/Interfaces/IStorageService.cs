using admin.Core.Model;

namespace admin.Core.Interfaces;

public interface IStorageService
{
    Task SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct);
    Task<TokenInfo?> GetTokenAsync(CancellationToken ct);
    Task ClearTokenAsync(CancellationToken ct);

    ApplicationSettings? LoadSettings();
    Task SaveSettingsAsync(ApplicationSettings appSettings);
}
