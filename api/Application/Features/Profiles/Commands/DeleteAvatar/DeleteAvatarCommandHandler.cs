using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Application.Features.Profiles.Common;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.DeleteAvatar;

public class DeleteAvatarCommandHandler : IRequestHandler<DeleteAvatarCommand, Result<ProfileResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<DeleteAvatarCommandHandler> _logger;
    public DeleteAvatarCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<User> userRepository,
        IFileStorage fileStorage,
        ICurrentUserService currentUserService,
        ILogger<DeleteAvatarCommandHandler> logger)
    {
        _uow = uow;
        _userRepository = userRepository;
        _fileStorage = fileStorage;
        _currentUserService = currentUserService;
        _logger = logger;
    }
    public async Task<Result<ProfileResponse>> Handle(DeleteAvatarCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null || !userId.Value.Equals(request.UserId))
            return Result<ProfileResponse>.Failed(ErrorCode.Unauthorized, "Не удалось определить пользователя");

        var user = await _userRepository.FirstOrDefaultAsync(new UserByIdWithProfileSpec(request.UserId), ct);

        if (user == null)
            return Result<ProfileResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

        var profile = user.Profile;

        if (profile == null)
            return Result<ProfileResponse>.Failed(ErrorCode.NotFound, "Профиль пользователя не найден");

        _logger.LogInformation($"User: {user.Username}, Profile: {profile}");

        var avatarPath = _fileStorage.GetFilePath(profile.AvatarUrl);

        if (avatarPath == null)
            return Result<ProfileResponse>.Failed(ErrorCode.NotFound, "Аватар пользователя не найден");

        await _fileStorage.DeleteFileAsync(avatarPath, ct);

        return Result<ProfileResponse>.Success(new ProfileResponse(
            DisplayName: profile.DisplayName ?? user.Username,
            Bio: profile.Bio ?? "",
            AvatarUrl: "avatars/default-avatar.png",
            LastOnline: profile.LastOnline ?? user.CreatedAt
        ));
    }
}