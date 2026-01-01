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
                .MaximumLength(20).WithMessage("Логин или email должен быть не более 20 символов");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов");
        }
    }
}