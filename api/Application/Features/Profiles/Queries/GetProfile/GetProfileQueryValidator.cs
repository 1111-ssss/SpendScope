using FluentValidation;

namespace Application.Features.Profiles.GetProfile;

public class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
{
    public GetProfileQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Идентификатор пользователя обязателен")
            .GreaterThan(0).WithMessage("Идентификатор пользователя должен быть больше 0");
    }
}