using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.AppVersions;
using MediatR;

namespace Application.Features.AppVersions.GetAllVersions;

public class GetAllVersionsQueryHandler : IRequestHandler<GetAllVersionsQuery, Result<GetAllVersionsResponse>>
{
    private readonly IBaseRepository<AppVersion> _appVersionRepository;
    public GetAllVersionsQueryHandler(IBaseRepository<AppVersion> appVersionRepository)
    {
        _appVersionRepository = appVersionRepository;
    }
    public async Task<Result<GetAllVersionsResponse>> Handle(GetAllVersionsQuery request, CancellationToken cancellationToken)
    {
        var result = await _appVersionRepository.ListAsync(new AppVersionUploadedBySpec(), cancellationToken);

        var grouped = result
            .GroupBy(x => x.Branch)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => new AppVersionResponse(
                    Branch: x.Branch,
                    Build: x.Build,
                    Changelog: x.Changelog,
                    UploadedAt: x.UploadedAt,
                    UploadedBy: x.UploadedBy?.ToString() ?? "Unknown"
                ))
            .ToList()
        );

        return Result<GetAllVersionsResponse>.Success(new GetAllVersionsResponse(
            grouped
        ));
    }
}