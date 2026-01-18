using admin.Core.Abstractions;

namespace admin.Core.Interfaces;

public interface IAuthService
{
    bool IsAuthenticated { get; }
    Result Login(string identifier, string password);
    Result Register(string username, string email, string password);
}
