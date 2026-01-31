using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Profiles.SearchProfiles;

public record SearchProfilesQuery(string Username, int Page, int PageSize) : IRequest<Result<ProfilesPageResponse>>;