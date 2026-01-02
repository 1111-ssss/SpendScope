using FluentValidation;

namespace Application.Features.Profiles.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.DisplayName)
                .MaximumLength(20).WithMessage("Название должно быть не более 20 символов");
            RuleFor(x => x.Bio)
                .MaximumLength(400).WithMessage("Описание должно быть не более 400 символов");
            RuleFor(f => f.Image)
                .Must(file => file!.Length > 0).WithMessage("Изображение пустое")
                .Must(file => new[] {".png", ".jpg", ".jpeg"}.Contains(Path.GetExtension(file!.FileName).ToLowerInvariant()))
                .WithMessage("Изображение должно иметь расширение .png, .jpg или .jpeg");
        }
    }
}