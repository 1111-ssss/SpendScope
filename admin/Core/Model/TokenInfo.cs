namespace admin.Core.Model;

public record TokenInfo(
    string JwtToken,
    string RefreshToken,
    DateTime ExpiresAt
);