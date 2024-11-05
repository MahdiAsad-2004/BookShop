using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Infrastructure.Identity
{
    internal class PasswordHasher : IPasswordHasher
    {
        public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, providedPassword) != PasswordVerificationResult.Failed;
        }
    }
}
