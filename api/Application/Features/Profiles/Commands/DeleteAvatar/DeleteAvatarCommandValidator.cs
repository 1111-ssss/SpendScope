using FluentValidation;

namespace Application.Features.Profiles.DeleteAvatar;

public class DeleteAvatarCommandValidator : AbstractValidator<DeleteAvatarCommand>
{
    public DeleteAvatarCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Идентификатор пользователя обязателен")
            .GreaterThan(0).WithMessage("Идентификатор пользователя должен быть больше 0");
    }
}