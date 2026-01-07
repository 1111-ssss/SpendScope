using FluentValidation;

namespace Application.Features.Follows.GetFollowing
{
    public class GetFollowingQueryValidator : AbstractValidator<GetFollowingQuery>
    {
        public GetFollowingQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Идентификатор пользователя обязателен")
                .GreaterThan(0).WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }
}