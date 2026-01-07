using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.AppVersions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AppVersions.DeleteVersion;

public class DeleteVersionCommandHandler : IRequestHandler<DeleteVersionCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<AppVersion> _appVersionRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<DeleteVersionCommandHandler> _logger;
    public DeleteVersionCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<AppVersion> appVersionRepository,
        IFileStorage fileStorage,
        ILogger<DeleteVersionCommandHandler> logger)
    {
        _uow = uow;
        _appVersionRepository = appVersionRepository;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteVersionCommand request, CancellationToken ct)
    {
        var appVer = await _appVersionRepository.FirstOrDefaultAsync(new AppVersionExistsSpec(request.Branch, request.Build), ct);
        if (appVer == null)
            return Result.Failed(ErrorCode.BadRequest, "Версия не существует");

        var safeBranch = Path.GetFileName(request.Branch);
        var safeBuild = Path.GetFileName(request.Build.ToString());

        await _fileStorage.DeleteFileAsync(Path.Combine("app", safeBranch, safeBuild), ct);

        await _appVersionRepository.DeleteAsync(appVer, ct);

        try
        {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении версии из базы данных");
            return Result.Failed(ErrorCode.InternalServerError, "Ошибка при удалении версии из базы данных");
        }

        return Result.Success();
    }
}