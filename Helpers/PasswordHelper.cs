using System;
using System.Security.Cryptography;
using System.Text;

namespace ValpeVerkkokauppa.Helpers
{
    public static class PasswordHelper
    {
        public static string GenerateSalt(int size = 16)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public static string HashPassword(string password, string salt)
        {
            var sha256 = SHA256.Create();
            var combinedPassword = password + salt;
            var bytes = Encoding.UTF8.GetBytes(combinedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var hash = HashPassword(enteredPassword, storedSalt);
            return hash == storedHash;
        }
    }
}
