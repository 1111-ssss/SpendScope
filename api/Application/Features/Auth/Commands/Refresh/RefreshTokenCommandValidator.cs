using FluentValidation;

namespace Application.Features.Auth.Refresh;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.JwtToken)
            .NotEmpty().WithMessage("JWT токен обязателен");
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Токен обновления обязателен");
    }
}