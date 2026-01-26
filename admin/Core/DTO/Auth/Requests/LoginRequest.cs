namespace admin.Core.DTO.Auth.Requests;

public record LoginRequest(
    string Identifier,
    string Password
);