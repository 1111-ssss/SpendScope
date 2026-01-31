namespace Application.Features.Profiles.Common;

public record ProfileResponse(string DisplayName, string Username, string AvatarUrl, string Bio, DateTime LastOnline);