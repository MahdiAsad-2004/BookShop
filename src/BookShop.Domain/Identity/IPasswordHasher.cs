using BookShop.Domain.Entities;

namespace BookShop.Domain.Identity
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    
    
    }

}
