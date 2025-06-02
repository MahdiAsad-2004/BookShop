using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Order : Entity<Guid> 
    {
        public Guid UserId { get; set; }
        public string? TrackingCode { get; set; }
        public string? UsedDiscountCode { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public float PackWeight { get; set; }
        public int DiscountPrice { get; set; }
        public int DeliveryPrice { get; set; }
        public int ProductPriceSummary { get; set; }
        public int TotlaPrice { get; set; }


        public User User { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public IList<OrderStatus> OrderStatuses { get; set; }
    
    }
}
