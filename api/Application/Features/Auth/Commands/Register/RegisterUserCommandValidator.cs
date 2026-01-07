using FluentValidation;

namespace Application.Features.Auth.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Логин обязателен")
            .MinimumLength(3).WithMessage("Логин должен быть не менее 3 символов")
            .MaximumLength(50).WithMessage("Логин должен быть не более 20 символов")
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Логин должен содержать только буквы и цифры");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен")
            .MinimumLength(3).WithMessage("Email должен быть не менее 3 символов")
            .MaximumLength(255).WithMessage("Email должен быть не более 20 символов")
            .EmailAddress().WithMessage(" Email должен быть корректным");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(3).WithMessage("Пароль должен быть не менее 3 символов")
            .MaximumLength(20).WithMessage("Пароль должен быть не более 20 символов")
            .Matches(@"^[\w!@#$%^&*()_+\-=\[\]{};':\\|,\.<>/?~`]+$")
            .WithMessage("Пароль должен содержать только буквы, цифры, знаки препинания испециальные символы");
    }
}