using admin.Core.Interfaces;
using admin.Infrastructure.Http.Clients;

namespace admin.Infrastructure.Services;

public class ApiService : IApiService
{
    public IAuthApi Auth { get; }
    public IHealthApi Health { get; }
    public IProfileApi Profile { get; }

    public ApiService(
        IAuthApi auth,
        IHealthApi health,
        IProfileApi profile

    )
    {
        Auth = auth;
        Health = health;
        Profile = profile;
    }
}
