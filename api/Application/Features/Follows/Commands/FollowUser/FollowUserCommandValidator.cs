using FluentValidation;

namespace Application.Features.Follows.FollowUser;

public class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
{
    public FollowUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Идентификатор пользователя обязателен")
            .GreaterThan(0).WithMessage("Идентификатор пользователя должен быть больше 0");
    }
}