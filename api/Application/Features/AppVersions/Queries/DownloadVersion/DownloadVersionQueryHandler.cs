using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Application.Common.Responses;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AppVersions.DownloadVersion;

public class DownloadVersionQueryHandler : IRequestHandler<DownloadVersionQuery, Result<FileDownloadResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<AppVersion> _appVersionRepository;
    private readonly ILogger<DownloadVersionQueryHandler> _logger;
    private readonly IFileStorage _fileStorage;
    public DownloadVersionQueryHandler(
        IUnitOfWork uow,
        IBaseRepository<AppVersion> appVersionRepository,
        IFileStorage fileStorage,
        ILogger<DownloadVersionQueryHandler> logger)
    {
        _uow = uow;
        _appVersionRepository = appVersionRepository;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<Result<FileDownloadResponse>> Handle(DownloadVersionQuery request, CancellationToken ct)
    {
        var filePath = _fileStorage.GetFilePath(
            Path.Combine("app", request.Branch, request.Build.ToString(), request.FileType == "apk" ? "SpendScope.apk" : "SpendScope.ipa")
        );

        if (filePath == null)
            return Result.NotFound("Версия не найдена");

        var contentType = Path.GetExtension(filePath) switch
        {
            ".apk" => "application/vnd.android.package-archive",
            ".ipa" => "application/octet-stream",
            _ => null
        };

        if (contentType == null)
            return Result.BadRequest("Неизвестный тип файла");

        return Result<FileDownloadResponse>.Success(new FileDownloadResponse(
            FilePath: filePath,
            ContentType: contentType,
            FileName: $"SpendScope.{Path.GetExtension(filePath)}"
        ));
    }
}