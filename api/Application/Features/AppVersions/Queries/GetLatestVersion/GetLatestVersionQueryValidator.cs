using FluentValidation;

namespace Application.Features.AppVersions.GetLatestVersion
{
    public class UploadVersionCommandValidator : AbstractValidator<GetLatestVersionQuery>
    {
        public UploadVersionCommandValidator()
        {
            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Ветка обязательна")
                .MinimumLength(3).WithMessage("Ветка должна быть не менее 3 символов")
                .MaximumLength(20).WithMessage("Ветка должна быть не более 20 символов");
        }
    }
}