using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Follows;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Follows.FollowUser
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<FollowUserCommandHandler> _logger;
        public FollowUserCommandHandler(
            IUnitOfWork uow,
            IBaseRepository<Follow> followRepository,
            ICurrentUserService currentUserService,
            ILogger<FollowUserCommandHandler> logger)
        {
            _uow = uow;
            _followRepository = followRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }
        public async Task<Result> Handle(FollowUserCommand request, CancellationToken ct)
        {
            var currentUserId = _currentUserService.GetUserId();
            if (currentUserId == null)
                return Result.Failed(ErrorCode.Unauthorized, "Не удалось определить пользователя");

            if (currentUserId.Value == request.UserId)
                return Result.Failed(ErrorCode.BadRequest, "Нельзя подписаться на самого себя");

            var existingFollow = await _followRepository.FirstOrDefaultAsync(
                new FollowExistsSpec(currentUserId.Value, request.UserId),
                ct);

            if (existingFollow != null)
            {
                return Result.Success();
            }

            var follow = Follow.Create(currentUserId.Value, request.UserId);
            await _followRepository.AddAsync(follow, ct);

            try
            {
                await _uow.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении подписки на пользователя");
                return Result.Failed(ErrorCode.InternalServerError, "Ошибка при добавлении подписки на пользователя");
            }

            return Result.Success();
        }
    }
}