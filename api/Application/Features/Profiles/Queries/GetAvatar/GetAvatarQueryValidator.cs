using FluentValidation;

namespace Application.Features.Profiles.GetAvatar;

public class GetAvatarQueryValidator : AbstractValidator<GetAvatarQuery>
{
    public GetAvatarQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Идентификатор пользователя обязателен")
            .GreaterThan(0).WithMessage("Идентификатор пользователя должен быть больше 0");
    }
}