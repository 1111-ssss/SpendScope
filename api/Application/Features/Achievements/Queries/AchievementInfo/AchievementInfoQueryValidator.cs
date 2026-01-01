using FluentValidation;

namespace Application.Features.Achievements.AchievementInfo
{
    public class AchievementInfoQueryValidator : AbstractValidator<AchievementInfoQuery>
    {
        public AchievementInfoQueryValidator()
        {
            RuleFor(a => a.AchievementId)
                .NotEmpty().WithMessage("Айди достижения не может быть пустым")
                .GreaterThan(0).WithMessage("Айдо должен быть больше 0");
        }
    }
}