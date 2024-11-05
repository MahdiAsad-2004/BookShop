using BookShop.Domain.Entities;

namespace BookShop.Domain.Identity
{
    public interface IPasswordHasher
    {
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    }

}
