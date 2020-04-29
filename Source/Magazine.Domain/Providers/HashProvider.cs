using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Infotecs.Magazine.Domain.Providers
{
    /// <summary>
    /// Генератор хеша.
    /// </summary>
    public class HashProvider
    {
        /// <summary>
        /// Генерация соли.
        /// </summary>
        /// <returns>Строка в формате Base64.</returns>
        public string GetSalt()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        /// <summary>
        /// Генерация хеша.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <param name="addSalt">Признак добавления соли.</param>
        /// <returns>Хеш на основе алгоритма хеширования SHA-256.</returns>
        public string GetHash(string input, bool addSalt = true)
        {
            var hashBytes = SHA256.Create().ComputeHash
                (Encoding.UTF8.GetBytes(addSalt ? input + GetSalt() : input));

            return hashBytes.Aggregate
                (new StringBuilder(),
                (result, next) => result.Append(next.ToString("x2")),
                totalResult => totalResult.ToString());
        }

        /// <summary>
        /// Генерация хеша.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <param name="salt">Соль.</param>
        /// <returns>Хеш на основе алгоритма хеширования SHA-256.</returns>
        public string GetHash(string input, string salt)
        {
            return GetHash(input + salt, false);
        }
    }
}
