using FluentValidation;

namespace Application.Features.Achievements.AchievementIcon
{
    public class AchievementIconQueryValidator : AbstractValidator<AchievementIconQuery>
    {
        public AchievementIconQueryValidator()
        {
            RuleFor(a => a.AchievementId)
                .NotEmpty().WithMessage("Айди достижения не может быть пустым")
                .GreaterThan(0).WithMessage("Айдо должен быть больше 0");
        }
    }
}