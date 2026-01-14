using Domain.Abstractions.Interfaces;
using Domain.Abstractions.Result;
using Domain.ValueObjects;

namespace Domain.Entities;

public class RefreshToken : IAggregateRoot
{
    public EntityId<RefreshToken> Id { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public string? CreatedByIp { get; private set; }
    // public bool IsRevoked { get; private set; }
    public EntityId<User> UserId { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    private RefreshToken() { }

    public static RefreshToken Create(
        string token,
        DateTime expires,
        string ipAddress,
        EntityId<User> userId)
    {
        return new RefreshToken
        {
            Token = token,
            ExpiresAt = expires,
            CreatedByIp = ipAddress,
            UserId = userId
        };
    }
    
    // public void Revoke()
    // {
    //     IsRevoked = true;
    // }

    public Result<string> Validate(string token, string? ipAddress = null)
    {
        if (IsExpired)
            return Result.BadRequest("Токен обновления устарел");
        // if (IsRevoked)
        //     return Result.BadRequest("Токен обновления был отозван");
        if (token != Token)
            return Result.BadRequest("Токен не совпадает");
        if (ipAddress != null && ipAddress != CreatedByIp)
            return Result.BadRequest("IP адрес не совпадает");
        return Result.Success();
    }
}