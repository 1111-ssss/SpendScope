using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.AppVersions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AppVersions.GetLatestVersion
{

    public class GetLatestVersionQueryHandler : IRequestHandler<GetLatestVersionQuery, Result<AppVersionResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<AppVersion> _appVersionRepository;
        private readonly ILogger<GetLatestVersionQueryHandler> _logger;
        public GetLatestVersionQueryHandler(
            IUnitOfWork uow,
            IBaseRepository<AppVersion> appVersionRepository,
            ILogger<GetLatestVersionQueryHandler> logger)
        {
            _uow = uow;
            _appVersionRepository = appVersionRepository;
            _logger = logger;
        }

        public async Task<Result<AppVersionResponse>> Handle(GetLatestVersionQuery request, CancellationToken ct)
        {
            var appVer = await _appVersionRepository.FirstOrDefaultAsync(new AppVersionByBranchSpec(request.Branch), ct);

            if (appVer == null)
                return Result<AppVersionResponse>.Failed(ErrorCode.BadRequest, "Версия не найдена");

            return Result<AppVersionResponse>.Success(new AppVersionResponse(
                Branch: appVer.Branch,
                Build: appVer.Build,
                Changelog: appVer.Changelog,
                UploadedAt: appVer.UploadedAt,
                UploadedBy: appVer.UploadedBy.ToString() ?? "Unknown"
            ));
        }
    }
}