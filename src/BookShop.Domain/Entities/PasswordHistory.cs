using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class PasswordHistory : Entity<Guid>
    {
        public string PasswordHash { get; set; }
        public Guid UserId { get; set; }



        public User User { get; set; }

    }
}
