using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Domain.Abstractions.Result;
using Application.Abstractions.Auth;

namespace Application.Service.Auth.Helpers
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
            try
            {
                var salt = RandomNumberGenerator.GetBytes(SaltLength);
                var pwdBytes = Encoding.UTF8.GetBytes(password);
                var argon2 = new Argon2id(_pepper != null ? Combine(pwdBytes, _pepper) : pwdBytes)
                {
                    Salt = salt,
                    DegreeOfParallelism = Parallelism,
                    MemorySize = MemoryCostKb,
                    Iterations = Iterations
                };
                var hash = argon2.GetBytes(HashLength);
                
                var result = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
                return Result<string>.Success(result);
            }
            catch
            {
                return Result<string>.Failed(ErrorCode.InternalServerError, "Ошибка хеширования");
            }
        }

        public bool Verify(string password, string hash)
        {
            if (string.IsNullOrEmpty(hash)) return false;
            try
            {
                var parts = hash.Split(':');
                if (parts.Length != 2) return false;
                var salt = Convert.FromBase64String(parts[0]);
                var hashBytes = Convert.FromBase64String(parts[1]);
                var pwdBytes = Encoding.UTF8.GetBytes(password);
                var argon2 = new Argon2id(_pepper != null ? Combine(pwdBytes, _pepper) : pwdBytes)
                {
                    Salt = salt,
                    DegreeOfParallelism = Parallelism,
                    MemorySize = MemoryCostKb,
                    Iterations = Iterations
                };
                var computed = argon2.GetBytes(hashBytes.Length);
                return CryptographicOperations.FixedTimeEquals(computed, hashBytes);
            }
            catch
            {
                return false;
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
    }
}