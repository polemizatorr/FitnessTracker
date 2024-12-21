using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace FitnessTracker.WebAPI.Cryptography
{
    public class PasswordHasher
    {
        // Hash the password using PBKDF2
        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Use PBKDF2 to hash the password with the salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000))  // 100,000 iterations
            {
                byte[] hash = pbkdf2.GetBytes(32); // 256-bit hash
                return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
            }
        }

        // Verify the password against the stored hash
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            // Convert the stored salt from Base64 to byte[]
            byte[] salt = Convert.FromBase64String(storedSalt);

            // Hash the entered password using the stored salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 100000))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 256-bit hash
                                                   // Compare the generated hash with the stored hash (in Base64 format)
                return Convert.ToBase64String(hash) == storedHash;
            }
        }
    }
}
