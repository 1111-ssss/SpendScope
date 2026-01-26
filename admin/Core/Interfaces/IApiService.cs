using admin.Infrastructure.Http.Clients;

namespace admin.Core.Interfaces;

public interface IApiService
{
    IAuthApi Auth { get; }
    IHealthApi Health { get; }
    IProfileApi Profile { get; }
}
