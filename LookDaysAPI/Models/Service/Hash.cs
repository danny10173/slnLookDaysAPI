using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;


namespace LookDaysAPI.Models.Service
{
    public class Hash
    {
        private static string SaltGenerator()
        {
            byte[] RNDbytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(RNDbytes);
                return Convert.ToBase64String(RNDbytes);
            }
        }

        private static string HashPassword(string value,string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(valueBytes);
        }

        public static string HashPassword(string password)
        {
            var salt = SaltGenerator();
            var hash = HashPassword(password, salt);
            var result = $"{salt}.{hash}";
            return result;
        }

        private static bool Validate(string password, string salt, string hash) => HashPassword(password, salt) == hash;

        public static bool VerifyHashedPassword(string password, string storePassword)
        {
            var parts = storePassword.Split('.');
            if(parts.Length != 2)
            {
                return false;
            }
            var salt = parts[0];
            var hash = parts[1];

            return Validate(password, salt, hash); ;
        }
    }
}
