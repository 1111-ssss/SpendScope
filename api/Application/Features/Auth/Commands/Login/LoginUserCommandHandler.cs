using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Auth;
using Application.Abstractions.Auth;
using MediatR;

namespace Application.Features.Auth.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(
        IUnitOfWork uow,
        IJwtGenerator jwtGenerator,
        IBaseRepository<User> userRepository,
        IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new UserByUsernameOrEmailSpec(request.Identifier), ct);

        if (user == null)
            return Result.BadRequest("Неверный логин или пароль");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash).IsSuccess)
            return Result.BadRequest("Неверный логин или пароль");

        var result = _jwtGenerator.GenerateToken(user);
        if (!result.IsSuccess)
            return Result.InternalServerError("Ошибка генерации токена");
            
        return result;
    }
}