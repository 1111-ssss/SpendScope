using admin.Core.Model;
using admin.Core.DTO.Auth.Responses;

namespace admin.Core.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    string? UserName { get; }
    string? UserId { get; }
    Task LoginAsync(TokenInfo tokenInfo);
    Task LoginAsync(AuthResponse response);
    Task LogoutAsync();
    event EventHandler? UserStateChanged;
}
