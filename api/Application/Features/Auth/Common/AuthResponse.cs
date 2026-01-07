namespace Application.Features.Auth;

public record AuthResponse(string JwtToken = default!, DateTime ExpiresAt = default!);