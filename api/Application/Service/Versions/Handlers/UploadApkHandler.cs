using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Service.Versions.Handlers
{
    public class UploadApkHandler
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<AppVersion> _appVersions;
        private readonly string _apkPath;
        public UploadApkHandler(IUnitOfWork db, IRepository<AppVersion> appVersions, IConfiguration config)
        {
            _db = db;
            _appVersions = appVersions;
            _apkPath = config.GetValue<string>("AppStorage:ApkPath") ?? "";
            if (!Directory.Exists(_apkPath))
                throw new ArgumentException("Путь к APK не указан в конфигурации");
        }
        public async Task<Result<UploadVersionResponse>> Handle(UploadVersionRequest request, IFormFile file, CancellationToken ct = default)
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

            var versionPath = Path.GetFullPath(Path.Combine(_apkPath, safeBranch, safeBuild));

            if (!versionPath.StartsWith(Path.GetFullPath(_apkPath), StringComparison.OrdinalIgnoreCase))
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
            await _db.SaveChangesAsync(ct);

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