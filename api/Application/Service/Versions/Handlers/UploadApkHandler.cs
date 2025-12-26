using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Service.Versions.Handlers
{
    public class UploadApkHandler
    {
        private readonly ICustomLogger<UploadApkHandler> _logger;
        private readonly IUnitOfWork _db;
        private readonly IRepository<AppVersion> _appVersions;
        public UploadApkHandler(ICustomLogger<UploadApkHandler> logger, IUnitOfWork db, IRepository<AppVersion> appVersions)
        {
            _logger = logger;
            _db = db;
            _appVersions = appVersions;
        }
        public async Task<Result<UploadVersionResponse>> Handle(UploadVersionRequest request, IFormFile file, string apkPath, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Branch) || string.IsNullOrWhiteSpace(request.Build))
                return Result<UploadVersionResponse>.Failed(ErrorCode.BadRequest, "branch и build обязательны");

            if (file == null || file.Length == 0)
                return Result<UploadVersionResponse>.Failed(ErrorCode.BadRequest, "Файл не загружен");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".apk")
                return Result<UploadVersionResponse>.Failed(ErrorCode.BadRequest, "Файл должен иметь расширение .apk");
            var safeBranch = Path.GetFileName(request.Branch);
            var safeBuild = Path.GetFileName(request.Build);

            var versionPath = Path.GetFullPath(Path.Combine(apkPath, safeBranch, safeBuild));

            if (!versionPath.StartsWith(Path.GetFullPath(apkPath), StringComparison.OrdinalIgnoreCase))
                return Result<UploadVersionResponse>.Failed(ErrorCode.BadRequest, "Недопустимые параметры branch или build");

            Directory.CreateDirectory(versionPath);

            var apkFilePath = Path.Combine(versionPath, "SpendScope.apk");

            await using (var stream = new FileStream(apkFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var appVersion = AppVersion.Create(
                branch: request.Branch,
                build: int.Parse(request.Build),
                uploadedBy: request.UploadedBy,
                changelog: request.Changelog
            );

            await _appVersions.AddAsync(appVersion, ct);

            try {
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при сохранении версии в базе данных", ex);
                return Result<UploadVersionResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при сохранении версии в базе данных");
            }

            var downloadUrl = $"/api/appversion/download/apk/{request.Branch}/{request.Build}";

            return Result<UploadVersionResponse>.Success(new UploadVersionResponse(
                Build: appVersion.Build,
                DownloadUrl: downloadUrl,
                Changelog: appVersion.Changelog,
                UploadedAt: appVersion.UploadedAt,
                UploadedBy: appVersion.UploadedBy.ToString() ?? "Unknown"
            ));
        }
    }
}