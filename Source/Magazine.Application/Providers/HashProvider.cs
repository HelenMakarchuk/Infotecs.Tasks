using Magazine.Application.Contracts.Provider;
using Serilog;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Magazine.Application.Providers
{
    public class HashProvider : IHashProvider
    {
        ILogger _logger;

        public HashProvider(ILogger logger)
        {
            _logger = logger;
        }

        public string GetSalt()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public string GetHash(string password, bool addSalt = true)
        {
            var hashBytes = SHA256.Create().ComputeHash
                (Encoding.UTF8.GetBytes(addSalt ? password + GetSalt() : password));

            return hashBytes.Aggregate
                (new StringBuilder(),
                (result, next) => result.Append(next.ToString("x2")),
                totalResult => totalResult.ToString());
        }

        public string GetHash(string password, string salt)
        {
            return GetHash(password + salt, false);
        }
    }
}
