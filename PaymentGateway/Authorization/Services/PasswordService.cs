using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PaymentGateway.Authorization.Services
{
    public class PasswordService
    {
        public virtual Password GenerateHashedPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hashed = KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                10000,
                256 / 8);

            return new Password { Hash = Convert.ToBase64String(hashed), Salt = Convert.ToBase64String(salt) };
        }

        public virtual bool IsPasswordValid(string password, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);

            byte[] hashed = KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                10000,
                256 / 8);

            string hashedPassword = Convert.ToBase64String(hashed);
            return hashedPassword == storedHash;
        }
    }
}
