using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class CartItem : Entity<Guid> 
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        

        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}
