using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class OrderItem : Entity<Guid> 
    {
        public int Quantity { get; set; }
        public string Product_Title { get; set; }
        public int Product_Price { get; set; }
        public int Product_PurchasePrice { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        

        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
