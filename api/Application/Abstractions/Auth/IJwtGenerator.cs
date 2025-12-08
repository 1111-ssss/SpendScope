using System;

namespace Application.Abstractions.Auth
{
    public interface IJwtGenerator
    {
        string GenerateToken(int userId, string username, bool isAdmin = false);
    }
}
