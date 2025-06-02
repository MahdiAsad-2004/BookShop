using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Cart : Entity<Guid> 
    {
        public Guid UserId { get; set; }
        
        public User User { get; set; }
        public IList<CartItem> CartItems { get; set; }
    
    }
}
