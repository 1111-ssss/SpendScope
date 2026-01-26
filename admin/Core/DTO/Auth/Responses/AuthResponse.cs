namespace admin.Core.DTO.Auth.Responses;

public record AuthResponse(
    string JwtToken,
    DateTime ExpiresAt,
    string RefreshToken
);