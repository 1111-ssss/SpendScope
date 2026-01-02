using FluentValidation;

namespace Application.Features.Follows.GetFollowers
{
    public class GetFollowersQueryValidator : AbstractValidator<GetFollowersQuery>
    {
        public GetFollowersQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Идентификатор пользователя обязателен")
                .GreaterThan(0).WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }
}