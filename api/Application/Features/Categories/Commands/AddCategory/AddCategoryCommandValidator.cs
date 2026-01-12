using FluentValidation;

namespace Application.Features.Categories.AddCategory;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .Length(3, 100).WithMessage("Название должно быть от 3 до 100 символов")
            .Matches("^[a-zA-Zа-яА-Я0-9 _-]+$").WithMessage("Название содержит недопустимые символы");
        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Описание не может быть длиннее 100 символов")
            .When(x => !string.IsNullOrEmpty(x.Description));
        RuleFor(x => x.IconFile)
            .Must(x => x!.Length > 0).WithMessage("Файл изображения не может быть пустым")
            .Must(x => x!.ContentType == "image/png" || x.ContentType == "image/jpeg" || x.ContentType == "image/jpg")
            .WithMessage("Файл изображения должен быть в формате png, jpg или jpeg")
            .When(x => x.IconFile != null);
    }
}