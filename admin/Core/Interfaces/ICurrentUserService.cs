using admin.Core.Model;
using admin.Features.Auth.DTO.Responses;

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
