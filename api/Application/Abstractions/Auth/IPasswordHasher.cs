using Domain.Abstractions.Result;

namespace Application.Abstractions.Auth;

public interface IPasswordHasher
{
    Result<string> Hash(string password);
    Result Verify(string password, string hash);
}