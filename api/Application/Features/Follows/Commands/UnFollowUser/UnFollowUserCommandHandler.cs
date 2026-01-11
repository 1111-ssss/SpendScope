using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Follows;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Follows.UnFollowUser;
    public class UnFollowUserCommandHandler : IRequestHandler<UnFollowUserCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UnFollowUserCommandHandler> _logger;
        public UnFollowUserCommandHandler(
            IUnitOfWork uow,
            IBaseRepository<Follow> followRepository,
            ICurrentUserService currentUserService,
            ILogger<UnFollowUserCommandHandler> logger)
        {
            _uow = uow;
            _followRepository = followRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }
        public async Task<Result> Handle(UnFollowUserCommand request, CancellationToken ct)
        {
            var currentUserId = _currentUserService.GetUserId();
            if (currentUserId == null)
                return Result.Unauthorized("Не удалось определить пользователя");

            if (currentUserId.Value == request.UserId)
                return Result.BadRequest("Нельзя отписаться от самого себя");

            var existingFollow = await _followRepository.FirstOrDefaultAsync(new FollowExistsSpec(currentUserId.Value, request.UserId), ct);

            if (existingFollow == null)
            {
                return Result.Success();
            }

            await _followRepository.DeleteAsync(existingFollow, ct);

            try
            {
                await _uow.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении подписки пользователя");
                return Result.InternalServerError("Ошибка при удалении подписки пользователя");
            }

            return Result.Success();
        }
    }