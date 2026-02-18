using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Features.Profiles.Common;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.SearchProfiles;

public class SearchProfilesQueryHandler : IRequestHandler<SearchProfilesQuery, Result<ProfilesPageResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<User> _usersRepository;
    private readonly ILogger<SearchProfilesQueryHandler> _logger;
    public SearchProfilesQueryHandler(IUnitOfWork unitOfWork, IBaseRepository<User> usersRepository, ILogger<SearchProfilesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _usersRepository = usersRepository;
        _logger = logger;
    }
    public async Task<Result<ProfilesPageResponse>> Handle(SearchProfilesQuery request, CancellationToken ct)
    {
        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? 10;

        var profiles = await _usersRepository.ListAsync(
            new GetProfilesByNameSpec(
                request.Username ?? "",
                page,
                pageSize
            ), ct);
        
        var totalCount = await _usersRepository.CountAsync(
            new GetProfilesByNameSpec(
                request.Username ?? "",
                1,
                int.MaxValue
            ), ct);

        var profilesResponse = profiles.Select(
            u => new ProfileResponse(
                DisplayName: u.Profile.DisplayName ?? u.Username,
                Username: u.Username,
                AvatarUrl: u.Profile.AvatarUrl ?? "avatars/default-avatar.png",
                Bio: u.Profile.Bio ?? string.Empty,
                LastOnline: u.Profile.LastOnline ?? DateTime.MinValue
            )
        ).ToList();

        return Result<ProfilesPageResponse>.Success(new ProfilesPageResponse(
            TotalCount: totalCount,
            CurrentPage: page,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling((double)totalCount / pageSize),
            Items: profilesResponse
        ));
    }
}