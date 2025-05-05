using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace textforum.logic.helpers
{
    public static class PasswordHashingHelper
    {
        private static int keySize = 64;
        private static int iterations = 350000;
        private static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public static (string passwordHashed, string passwordSalt) GetPasswordHash(string password)
        {
            var result = hashPassword(password, out var salt);
            var saltString = Convert.ToHexString(salt);
            return (passwordHashed: result, passwordSalt: saltString);
        }

        private static string hashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
            salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }

        public static bool PasswordIsValid(string password, string passwordHash, string salt)
        {
            return verifyPassword(password, passwordHash, Convert.FromHexString(salt));
        }

        private static bool verifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
