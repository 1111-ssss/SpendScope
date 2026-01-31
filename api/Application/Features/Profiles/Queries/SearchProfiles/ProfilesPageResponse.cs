using Application.Features.Profiles.Common;

namespace Application.Features.Profiles.SearchProfiles;

public record ProfilesPageResponse(int TotalCount, int PageSize, int CurrentPage, int TotalPages, List<ProfileResponse> Items);