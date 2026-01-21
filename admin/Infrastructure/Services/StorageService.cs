using admin.Core.Enums;
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
    private const string TokenFileName = "token_info.dat";
    private static readonly string TokenFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SpendScopeAdmin",
        TokenFileName
    );
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("app_specific-salt_123");
    private readonly ILogger<StorageService> _logger;

    public StorageService(ILogger<StorageService> logger)
    {
        _logger = logger;
    }

    public async Task<TokenInfo?> GetTokenAsync(CancellationToken ct)
    {
        if (!File.Exists(TokenFilePath))
            //return Result.Failed(ResultErrorCode.ReadWrite, "Файл с токеном не найден");
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
                //return Result.Failed(ResultErrorCode.DeserializationFailed, "Ошибка десириализации");
                return null;

            //return Result<TokenInfo>.Success(tokenInfo);
            return tokenInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка чтения токена");
            //return Result.Failed(ResultErrorCode.Unknown, "Ошибка получения токена");
            return null;
        }
    }

    public async Task<Result> SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TokenFilePath)!);

            var json = JsonSerializer.Serialize(tokenInfo);
            var bytes = Encoding.UTF8.GetBytes(json);

            var protectedBytes = ProtectedData.Protect(
                bytes,
                Entropy,
                DataProtectionScope.CurrentUser
            );

            await File.WriteAllBytesAsync(TokenFilePath, protectedBytes, ct);

            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения токена: {ex.Message}");
            return Result.Failed("Не удалось сохранить токен");
        }
    }

    public async Task<Result> ClearTokenAsync(CancellationToken ct)
    {
        try
        {
            if (File.Exists(TokenFilePath))
            {
                await Task.Run(() => File.Delete(TokenFilePath), ct);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка очистки токена: {ex.Message}");
            return Result.Failed("Не удалось удалить токен");
        }
    }
}
