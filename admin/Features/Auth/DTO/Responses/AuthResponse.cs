namespace admin.Features.Auth.DTO.Responses;

public record AuthResponse(
    string JwtToken,
    DateTime ExpiresAt,
    string RefreshToken
);