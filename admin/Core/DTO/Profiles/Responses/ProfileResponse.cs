namespace admin.Core.DTO.Profiles.Responses;

public record ProfileResponse(
    int UserId,
    string DisplayName,
    string Username,
    string AvatarUrl,
    string Bio,
    DateTime LastOnline
);