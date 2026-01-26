namespace admin.Core.DTO.Profiles.Responses;

public record ProfileResponse(
    string DisplayName,
    string AvatarUrl,
    string Bio,
    DateTime LastOnline
);