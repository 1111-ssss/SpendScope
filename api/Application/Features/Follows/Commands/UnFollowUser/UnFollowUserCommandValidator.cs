using FluentValidation;

namespace Application.Features.Follows.UnFollowUser;
    public class UnFollowUserCommandValidator : AbstractValidator<UnFollowUserCommand>
    {
        public UnFollowUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Идентификатор пользователя обязателен")
                .GreaterThan(0).WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }