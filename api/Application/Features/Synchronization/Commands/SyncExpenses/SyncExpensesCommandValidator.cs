using FluentValidation;

namespace Application.Features.Synchronization.SyncExpenses;

public class SyncExpensesCommandValidator : AbstractValidator<SyncExpensesCommand>
{
    public SyncExpensesCommandValidator()
    {
        RuleFor(x => x.Expenses)
            .NotNull().WithMessage("Запрос не может быть пустым")
            .Must(x => x.Length > 0).WithMessage("Запрос не может быть пустым")
            .Must(x => x.Length <= 100).WithMessage("Максимальный размер запроса - 100 экспедитов");
    }
}