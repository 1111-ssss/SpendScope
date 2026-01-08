using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.Features.Logging.GetLogs;

public class GetLogsQueryValidator : AbstractValidator<GetLogsQuery>
{
    public GetLogsQueryValidator()
    {
        RuleFor(x => x.MinimalLevel)
            .NotEmpty().WithMessage("Минимальный уровень должен быть задан")
            .Must(x => Enum.TryParse<LogLevel>(x.ToString(), out _)).WithMessage("Минимальный уровень должен быть задан корректно");
        RuleFor(x => x.Level)
            .Must(x => x == null || Enum.TryParse<LogLevel>(x.ToString(), out _)).WithMessage("Уровень должен быть задан корректно")
            .When(x => x.Level != null);
        RuleFor(x => x.Page)
            .NotEmpty().WithMessage("Страница должна быть задана")
            .GreaterThan(0).WithMessage("Страница должна быть больше нуля");
        RuleFor(x => x.PageSize)
            .NotEmpty().WithMessage("Количество записей должно быть задано")
            .GreaterThan(0).WithMessage("Количество записей должно быть больше нуля")
            .LessThanOrEqualTo(100).WithMessage("Количество записей не должно превышать 100");
        RuleFor(x => x.OrderBy)
            .NotEmpty().WithMessage("Поле сортировки должно быть задано")
            .Must(x => new[] { "Timestamp", "Level" }.Contains(x)).WithMessage("Поле сортировки должно быть Timestamp или Level");
        RuleFor(x => x.IsDesc)
            .NotEmpty().WithMessage("Порядок сортировки должен быть задан");
    }
}