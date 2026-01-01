using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.AppVersions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AppVersions.DownloadVersion
{

    public class DownloadVersionQueryHandler : IRequestHandler<DownloadVersionQuery, Result<DownloadVersionResponse>>
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

        public async Task<Result<DownloadVersionResponse>> Handle(DownloadVersionQuery request, CancellationToken ct)
        {
            var filePath = _fileStorage.GetFilePath(Path.Combine(request.Branch, request.Build.ToString()));

            if (filePath == null)
                return Result<DownloadVersionResponse>.Failed(ErrorCode.NotFound, "Версия не найдена");

            return Result<DownloadVersionResponse>.Success(new DownloadVersionResponse(
                FilePath: filePath,
                FileType: Path.GetExtension(filePath)
            ));
        }
    }
}