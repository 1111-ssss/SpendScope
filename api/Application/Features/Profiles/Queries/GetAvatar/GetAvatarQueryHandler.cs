using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Application.Common.Responses;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.GetAvatar;

public class GetAvatarQueryHandler : IRequestHandler<GetAvatarQuery, Result<FileDownloadResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<GetAvatarQueryHandler> _logger;
    public GetAvatarQueryHandler(
        IUnitOfWork uow,
        IBaseRepository<User> userRepository,
        IFileStorage fileStorage,
        ILogger<GetAvatarQueryHandler> logger)
    {
        _uow = uow;
        _userRepository = userRepository;
        _fileStorage = fileStorage;
        _logger = logger;
    }
    public async Task<Result<FileDownloadResponse>> Handle(GetAvatarQuery request, CancellationToken ct)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new UserByIdWithProfileSpec(request.UserId), ct);

        if (user == null)
            return Result<FileDownloadResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

        var profile = user.Profile;

        if (profile == null)
            return Result<FileDownloadResponse>.Failed(ErrorCode.NotFound, "Профиль пользователя не найден");

        var avatarPath = _fileStorage.GetFilePath(profile.AvatarUrl, "avatars/default-avatar.png");

        if (avatarPath == null)
            return Result<FileDownloadResponse>.Failed(ErrorCode.NotFound, "Аватар пользователя не найден");

        return Result<FileDownloadResponse>.Success(new FileDownloadResponse(
            FilePath: avatarPath,
            ContentType: "image/png"
        ));
    }
}