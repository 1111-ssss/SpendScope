namespace admin.Core.DTO.Auth.Requests;

public record RefreshTokenRequest(
    string JwtToken,
    string RefreshToken
);