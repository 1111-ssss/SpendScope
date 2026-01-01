using Application.Features.AppVersions.DownloadVersion;
using FluentValidation;

namespace Application.Features.AppVersions.GetLatestVersion
{
    public class DownloadVersionQueryValidator : AbstractValidator<DownloadVersionQuery>
    {
        public DownloadVersionQueryValidator()
        {
            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Ветка обязательна")
                .MinimumLength(3).WithMessage("Ветка должна быть не менее 3 символов")
                .MaximumLength(20).WithMessage("Ветка должна быть не более 20 символов");
            RuleFor(x => x.Build)
                .NotEmpty().WithMessage("Билд обязателен")
                .GreaterThan(0).WithMessage("Билд должен быть больше 0");
        }
    }
}