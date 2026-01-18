using admin.Core.Abstractions;
using admin.Core.Interfaces;

namespace admin.Infrastructure.Services;

public class AuthService : IAuthService
{
    public bool IsAuthenticated { get; set; }
    public AuthService()
    {
        IsAuthenticated = false;
    }

    public Result Login(string identifier, string password)
    {
        throw new NotImplementedException();
    }

    public Result Register(string username, string email, string password)
    {
        throw new NotImplementedException();
    }
}
