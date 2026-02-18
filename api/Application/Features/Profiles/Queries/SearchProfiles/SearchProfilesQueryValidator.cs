using FluentValidation;

namespace Application.Features.Profiles.SearchProfiles;

public class SearchProfilesQueryValidator : AbstractValidator<SearchProfilesQuery>
{
    public SearchProfilesQueryValidator()
    {
        RuleFor(x => x.Username)
            .MaximumLength(50).WithMessage("Имя пользователя должно быть не более 50 символов");
        RuleFor(x => x.Page)
            .NotEmpty().WithMessage("Номер страницы не может быть пустым")
            .GreaterThan(0).WithMessage("Номер страницы должен быть больше 0");
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Размер страницы должен быть больше 0")
            .LessThanOrEqualTo(100).WithMessage("Размер страницы должен быть не более 100");
    }
}