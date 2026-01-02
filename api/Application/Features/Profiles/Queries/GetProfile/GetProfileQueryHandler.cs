using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Features.Profiles.Common;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.GetProfile
{

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Profile> _profileRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly ILogger<GetProfileQueryHandler> _logger;
        public GetProfileQueryHandler(
            IUnitOfWork uow,
            IBaseRepository<Profile> profileRepository,
            IBaseRepository<User> userRepository,
            ILogger<GetProfileQueryHandler> logger)
        {
            _uow = uow;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<Result<ProfileResponse>> Handle(GetProfileQuery request, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, ct);

            if (user == null)
                return Result<ProfileResponse>.Failed(ErrorCode.NotFound, "Пользователь не найден");

            // var profile = await _profileRepository.GetByIdAsync(user.Id, ct);
            var profile = user.Profile;

            if (profile == null)
                return Result<ProfileResponse>.Failed(ErrorCode.NotFound, "Профиль пользователя не найден");

            _logger.LogInformation($"User: {user.Username}, Profile: {profile}");

            return Result<ProfileResponse>.Success(new ProfileResponse(
                DisplayName: profile.DisplayName ?? user.Username,
                AvatarUrl: profile.AvatarUrl ?? "avatars/default-avatar.png",
                Bio: profile.Bio ?? "",
                LastOnline: profile.LastOnline ?? user.CreatedAt
            ));
        }
    }
}