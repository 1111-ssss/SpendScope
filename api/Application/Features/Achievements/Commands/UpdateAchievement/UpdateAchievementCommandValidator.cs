using FluentValidation;

namespace Application.Features.Achievements.UpdateAchievement
{
    public class UpdateAchievementCommandValidator : AbstractValidator<UpdateAchievementCommand>
    {
        public UpdateAchievementCommandValidator()
        {
            RuleFor(x => x.AchievementId)
                .NotEmpty().WithMessage("Айди достижения не может быть пустым")
                .GreaterThan(0).WithMessage("Айди должно быть больше 0");
            RuleFor(x => x.Name)
                .MinimumLength(3).WithMessage("Название должно быть не менее 3 символов")
                .MaximumLength(20).WithMessage("Название должно быть не более 20 символов")
                .When(x => !string.IsNullOrEmpty(x.Name));
            RuleFor(x => x.Description)
                .MinimumLength(3).WithMessage("Описание должно быть не менее 3 символов")
                .MaximumLength(256).WithMessage("Описание должно быть не более 256 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));
            RuleFor(f => f.Image)
                .Must(file => file == null || file!.Length > 0).WithMessage("Изображение пустое")
                .Must(file => file == null || new[] {".png", ".jpg", ".jpeg"}.Contains(Path.GetExtension(file!.FileName)?.ToLowerInvariant() ?? ""))
                .WithMessage("Изображение должно иметь расширение .png, .jpg или .jpeg")
                .When(x => x.Image != null);
        }
    }
}