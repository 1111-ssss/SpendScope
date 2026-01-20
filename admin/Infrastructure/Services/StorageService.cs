using admin.Core.Abstractions;
using admin.Core.Interfaces;
using admin.Core.Model;

namespace admin.Infrastructure.Services;

public class StorageService : IStorageService
{
    public Task<TokenInfo> GetTokenAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SaveTokenAsync(TokenInfo tokenInfo, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ClearTokenAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
