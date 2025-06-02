using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class OrderStatus : Entity<Guid> 
    {
        public Enums.OrderStatus Status { get; set; }
        public bool Done { get; set; }
        public DateTime? DoneDate { get; set; }
        public Guid OrderId { get; set; }

        public Order Order { get; set; }
    
    }
}
