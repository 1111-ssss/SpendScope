using FluentValidation;

namespace Application.Features.Auth.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Логин или email обязателен")
                .MinimumLength(3).WithMessage("Логин или email должен быть не менее 3 символов")
                .MaximumLength(50).WithMessage("Логин или email должен быть не более 20 символов");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(3).WithMessage("Пароль должен быть не менее 3 символов")
                .MaximumLength(20).WithMessage("Пароль должен быть не более 20 символов")
                .Matches(@"^[\w!@#$%^&*()_+\-=\[\]{};':\\|,\.<>/?~`]+$")
                .WithMessage("Пароль должен содержать только буквы, цифры, знаки препинания испециальные символы");
        }
    }
}