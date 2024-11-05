using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Favorite : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }


        public User User { get; set; }
        public Product Product { get; set; }
        
    }
}
