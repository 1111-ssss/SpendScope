namespace admin.Features.Auth.DTO.Requests;

public record RegisterRequest(
    string Username,
    string Email,
    string Password
);