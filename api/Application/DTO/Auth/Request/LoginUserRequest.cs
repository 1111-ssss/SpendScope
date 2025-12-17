using Domain.Enums;

namespace Application.DTO.Auth
{
    public record LoginUserRequest(string UsernameOrEmail, string Password, LoginMethod LoginMethod);
}