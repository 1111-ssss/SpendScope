namespace Application.Features.Profiles.Common;

public record ProfileResponse(int UserId, string DisplayName, string Username, string AvatarUrl, string Bio, DateTime LastOnline);