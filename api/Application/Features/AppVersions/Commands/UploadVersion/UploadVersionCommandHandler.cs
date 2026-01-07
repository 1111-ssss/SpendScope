using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.AppVersions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AppVersions.UploadVersion;

public class UploadVersionCommandHandler : IRequestHandler<UploadVersionCommand, Result<AppVersionResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<AppVersion> _appVersionRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<UploadVersionCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    public UploadVersionCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<AppVersion> appVersionRepository,
        IFileStorage fileStorage,
        ICurrentUserService currentUserService,
        ILogger<UploadVersionCommandHandler> logger)
    {
        _uow = uow;
        _appVersionRepository = appVersionRepository;
        _fileStorage = fileStorage;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<Result<AppVersionResponse>> Handle(UploadVersionCommand request, CancellationToken ct)
    {
        var appVer = await _appVersionRepository.FirstOrDefaultAsync(new AppVersionExistsSpec(request.Branch, request.Build), ct);
        if (appVer != null)
            return Result<AppVersionResponse>.Failed(ErrorCode.BadRequest, "Версия уже существует");

        var userId = _currentUserService.GetUserId();
        if (userId == null)
            return Result<AppVersionResponse>.Failed(ErrorCode.Unauthorized, "Не удалось определить пользователя");

        var safeBranch = Path.GetFileName(request.Branch);
        var safeBuild = Path.GetFileName(request.Build.ToString());

        var fileName = Path.GetExtension(request.File.FileName) switch
        {
            ".apk" => "SpendScope.apk",
            ".ipa" => "SpendScope.ipa",
            _ => null
        };
        if (fileName == null)
            return Result<AppVersionResponse>.Failed(ErrorCode.BadRequest, "Неизвестный тип файла");

        await _fileStorage.SaveFileAsync(
            request.File,
            Path.Combine("app", safeBranch, safeBuild),
            fileName,
            ct
        );

        var appVersion = AppVersion.Create(
            branch: request.Branch,
            build: request.Build,
            uploadedBy: userId.Value,
            changelog: request.Changelog
        );

        await _appVersionRepository.AddAsync(appVersion, ct);

        try
        {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при сохранении версии в базе данных");
            return Result<AppVersionResponse>.Failed(ErrorCode.InternalServerError, "Ошибка при сохранении версии в базе данных");
        }

        return Result<AppVersionResponse>.Success(new AppVersionResponse(
            Branch: appVersion.Branch,
            Build: appVersion.Build,
            Changelog: appVersion.Changelog,
            UploadedAt: appVersion.UploadedAt,
            UploadedBy: appVersion.UploadedBy.ToString() ?? "Unknown"
        ));
    }
}