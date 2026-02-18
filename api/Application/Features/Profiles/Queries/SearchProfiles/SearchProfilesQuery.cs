using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Profiles.SearchProfiles;

public record SearchProfilesQuery(string? Username = "", int? Page = 1, int? PageSize = 10) : IRequest<Result<ProfilesPageResponse>>;