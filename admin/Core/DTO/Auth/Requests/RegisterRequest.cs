namespace admin.Core.DTO.Auth.Requests;

public record RegisterRequest(
    string Username,
    string Email,
    string Password
);