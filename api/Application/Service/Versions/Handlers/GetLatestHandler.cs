using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;

namespace Application.Service.Versions.Handlers
{
    public class GetLatestHandler
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<AppVersion> _appVersions;
        public GetLatestHandler(IUnitOfWork db, IRepository<AppVersion> appVersions)
        {
            _db = db;
            _appVersions = appVersions;
        }
        public async Task<Result<GetLatestVersionResponse>> Handle(GetLatestVersionRequest request, CancellationToken ct = default)
        {
            var latest = await _appVersions.FindSingleAsync(new AppVersionByBranchSpecification(request.Branch), ct);

            if (latest == null)
                return Result<GetLatestVersionResponse>.Failed(ErrorCode.Forbidden, "Версия не найдена");

            var downloadUrl = $"/api/appversion/download/apk/{latest.Branch}/{latest.Build}";

            return Result<GetLatestVersionResponse>.Success(new GetLatestVersionResponse(
                Build: latest.Build,
                DownloadUrl: downloadUrl,
                Changelog: latest.Changelog,
                UploadedAt: latest.UploadedAt,
                UploadedBy: latest.UploadedBy.ToString() ?? "Unknown"
            ));
        }
    }
}