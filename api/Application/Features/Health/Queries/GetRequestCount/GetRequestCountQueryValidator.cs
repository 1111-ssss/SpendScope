using FluentValidation;

namespace Application.Features.Health.GetRequestCount;

public class GetStatsQueryValidator : AbstractValidator<GetRequestCountQuery>
{
    public GetStatsQueryValidator()
    {
        RuleFor(x => x.DateTime)
            .NotNull().WithMessage("Дата не может быть пустой")
            .GreaterThan(DateTime.UtcNow.AddDays(-1))
            .WithMessage("Дата должна быть в период сегодняшнего дня")
            .LessThan(DateTime.UtcNow.AddHours(1))
            .WithMessage("Дата должна быть в период сегодняшнего дня");
    }
}