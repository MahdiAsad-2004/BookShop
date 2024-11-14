using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Product_Discount : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid DiscountId { get; set; }

        public Product Product { get; set; }
        public Discount Discount { get; set; }
    
    }
}
