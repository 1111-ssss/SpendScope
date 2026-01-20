namespace admin.Core.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    string? UserName { get; }
    string? UserId { get; }
    void SetFromToken(string token);
}
