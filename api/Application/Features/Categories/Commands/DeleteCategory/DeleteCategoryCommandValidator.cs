using FluentValidation;

namespace Application.Features.Categories.DeleteCategory;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Не указан идентификатор категории")
            .GreaterThan(0).WithMessage("Неверный идентификатор категории");
    }
}