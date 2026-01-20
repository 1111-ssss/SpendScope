namespace admin.Features.Auth.DTO.Requests;

public record RefreshTokenRequest(
    string JwtToken,
    string RefreshToken
);