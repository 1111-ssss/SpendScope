using FluentValidation;

namespace Application.Features.AppVersions.UploadVersion
{
    public class UploadVersionCommandValidator : AbstractValidator<UploadVersionCommand>
    {
        public UploadVersionCommandValidator()
        {
            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Ветка обязательна")
                .MinimumLength(3).WithMessage("Ветка должна быть не менее 3 символов")
                .MaximumLength(20).WithMessage("Ветка должна быть не более 20 символов");
            RuleFor(x => x.Build)
                .NotEmpty().WithMessage("Билд обязателен")
                .GreaterThan(0).WithMessage("Билд должен быть больше 0");
            RuleFor(c => c.Changelog)
                .MaximumLength(256).WithMessage("Changelog должен быть не более 256 символов");
            RuleFor(f => f.File)
                .NotNull().WithMessage("Файл обязателен")
                .Must(file => file!.Length > 0).WithMessage("Файл пустой")
                .Must(file => new[] {".apk", ".ipa"}.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage("Файл должен иметь расширение .apk или .ipa");
                
        }
    }
}