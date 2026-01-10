namespace Application.Abstractions.Auth;

public interface ICurrentUserService
{
    int? GetUserId();
    bool IsAdmin();
    string GetUserIp();
}