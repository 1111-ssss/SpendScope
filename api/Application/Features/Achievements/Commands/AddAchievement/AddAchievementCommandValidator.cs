using FluentValidation;

namespace Application.Features.Achievements.AddAchievement;

public class AddAchievementCommandValidator : AbstractValidator<AddAchievementCommand>
{
    public AddAchievementCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название обязательное")
            .MinimumLength(3).WithMessage("Название должно быть не менее 3 символов")
            .MaximumLength(20).WithMessage("Название должно быть не более 20 символов");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание обязательное")
            .MinimumLength(3).WithMessage("Описание должно быть не менее 3 символов")
            .MaximumLength(256).WithMessage("Описание должно быть не более 256 символов");
        RuleFor(f => f.Image)
            .NotNull().WithMessage("Изображение обязательное")
            .Must(file => file!.Length > 0).WithMessage("Изображение пустое")
            .Must(file => new[] { ".png", ".jpg", ".jpeg" }.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
            .WithMessage("Изображение должно иметь расширение .png, .jpg или .jpeg");

    }
}