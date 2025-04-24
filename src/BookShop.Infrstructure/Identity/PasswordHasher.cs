using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace BookShop.Infrastructure.Identity
{
    internal class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                StringBuilder hashBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashBuilder.Append(b.ToString("x2"));
                }

                return hashBuilder.ToString();
            }
        }
 


      public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, providedPassword) != PasswordVerificationResult.Failed;
        }
    }
}
