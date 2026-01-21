using admin.Core.Model;

namespace admin.Core.Interfaces;

public interface IStorageService
{
    Task<Result> SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct);
    //Task<Result> GetTokenAsync();
    Task<TokenInfo> GetTokenAsync(CancellationToken ct);
    Task<Result> ClearTokenAsync(CancellationToken ct);
}
