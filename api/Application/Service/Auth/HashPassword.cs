using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Configuration;
using Domain.Model.Result;
using Application.Abstractions.Auth;

namespace Application.Service.Auth
{
    public sealed class Argon2PasswordHasher : IPasswordHasher
    {
        private const int SaltLength = 16;
        private const int HashLength = 32;
        private const int MemoryCostKb = 64 * 1024;
        private const int Iterations = 3;
        private const int Parallelism = 4;
        private readonly byte[]? _pepper;

        public Argon2PasswordHasher(IConfiguration config)
        {
            var pepperBase64 = config["Password__Pepper"];
            _pepper = string.IsNullOrEmpty(pepperBase64) ? null : Convert.FromBase64String(pepperBase64);
        }

        public Result<string> Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltLength);

            var config = new Argon2Config
            {
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = Iterations,
                MemoryCost = MemoryCostKb,
                Lanes = Parallelism,
                Threads = Parallelism,
                Password = Encoding.UTF8.GetBytes(password),
                Salt = salt,
                Secret = _pepper,
                AssociatedData = null,
                HashLength = HashLength
            };

            var hash = new Argon2(config).Hash();

            if (hash == null)
                return Result<string>.Failed(ErrorCode.InternalServerError, "Ошибка хеширования");

            return Result<string>.Success(hash.ToString()!);
        }

        public bool Verify(string password, string hash)
        {
            return Argon2.Verify(hash, Encoding.UTF8.GetBytes(password), Parallelism);
        }
    }
}