namespace admin.Features.Auth.DTO.Requests;

public record LoginRequest(
    string Identifier,
    string Password
);