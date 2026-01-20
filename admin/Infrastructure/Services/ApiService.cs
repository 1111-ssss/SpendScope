using admin.Core.Interfaces;
using admin.Infrastructure.Http.Clients;

namespace admin.Infrastructure.Services;

public class ApiService : IApiService
{
    public IAuthApi Auth { get; }

    public ApiService(
        IAuthApi auth
    )
    {
        Auth = auth;
    }
}
