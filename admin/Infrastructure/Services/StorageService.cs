using admin.Core.Interfaces;
using admin.Core.Model;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace admin.Infrastructure.Services;

public class StorageService : IStorageService
{
    private static readonly string BaseFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SpendScopeAdmin"
    );
    private static readonly string TokenFilePath = Path.Combine(
        BaseFilePath,
        "token_info.dat"
    );
    private static readonly string SettingsFilePath = Path.Combine(
        BaseFilePath,
        "app_settings.json"
    );
    private JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("admin.app_specific-salt_123");
    private readonly ILogger<StorageService> _logger;

    public StorageService(ILogger<StorageService> logger)
    {
        _logger = logger;
        Directory.CreateDirectory(Path.GetDirectoryName(TokenFilePath)!);
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
    }

    public async Task<TokenInfo?> GetTokenAsync(CancellationToken ct)
    {
        if (!File.Exists(TokenFilePath))
            return null;

        try
        {
            var encryptedBytes = await File.ReadAllBytesAsync(TokenFilePath, ct);

            var unprotectedBytes = ProtectedData.Unprotect(
                encryptedBytes,
                Entropy,
                DataProtectionScope.CurrentUser
            );

            var json = Encoding.UTF8.GetString(unprotectedBytes);
            var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(json);

            if (tokenInfo == null)
                return null;

            return tokenInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка чтения токена");
            return null;
        }
    }

    public async Task SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct)
    {
        try
        {
            var json = JsonSerializer.Serialize(tokenInfo);
            var bytes = Encoding.UTF8.GetBytes(json);

            var protectedBytes = ProtectedData.Protect(
                bytes,
                Entropy,
                DataProtectionScope.CurrentUser
            );

            await File.WriteAllBytesAsync(TokenFilePath, protectedBytes, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка сохранения токена");
        }
    }

    public async Task ClearTokenAsync(CancellationToken ct)
    {
        try
        {
            if (File.Exists(TokenFilePath))
            {
                await Task.Run(() => File.Delete(TokenFilePath), ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка очистки токена");
        }
    }

    public ApplicationSettings? LoadSettings()
    {
        if (!File.Exists(SettingsFilePath))
            return null;

        try
        {
            string json = File.ReadAllText(SettingsFilePath);
            return JsonSerializer.Deserialize<ApplicationSettings>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка загрузки настроек");
            return null;
        }
    }

    public async Task SaveSettingsAsync(ApplicationSettings appSettings)
    {
        try
        {
            string json = JsonSerializer.Serialize(
                appSettings,
                serializerOptions
            );

            await File.WriteAllTextAsync(SettingsFilePath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка сохранения настроек");
        }
    }
}
