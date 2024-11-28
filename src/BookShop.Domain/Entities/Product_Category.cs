
using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Product_Category : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsMain { get; set; }


        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
