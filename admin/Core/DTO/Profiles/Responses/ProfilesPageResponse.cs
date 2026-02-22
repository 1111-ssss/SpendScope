namespace admin.Core.DTO.Profiles.Responses;

public record ProfilesPageResponse(
    int TotalPages,
    int PageSize,
    int CurrentPage,
    List<ProfileResponse> Items
);