
using BookShop.Domain.Entities;
using System.Reflection.Emit;

namespace BookShop.IntegrationTest.Application.Discount.FakeData
{
    public class DiscountFakeData
    {
       
        public static Domain.Entities.Discount Create(Guid id, int? priority = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            string identifier = id.ToString().Substring(0, 2);
            Domain.Entities.Discount discount = new()
            {
                DiscountPercent = Random.Shared.Next(1, 100),
                DiscountPrice = null,
                EndDate = endDate,
                Id = id,
                Name = $"TestDiscount-{identifier}",
                Priority = priority ?? Random.Shared.Next(1, 30),
                StartDate = startDate,
            };
            return discount;
        }
        

        public static Domain.Entities.Discount Create(int? priority = null, DateTime? startDate = null, DateTime? endDate = null ,
            int? usedCount = null , int? maxUseCount = null)
        {
            Domain.Entities.Discount discount = Create(Guid.NewGuid(), priority, startDate, endDate);
            return discount;
        }


        public static Domain.Entities.Discount Create(int discountPrice, int? priority = null, DateTime? startDate = null, DateTime? endDate = null,
            int? usedCount = null, int? maxUseCount = null)
        {
            Domain.Entities.Discount discount = Create(priority, startDate, endDate,usedCount , maxUseCount);
            discount.DiscountPrice = discountPrice;
            discount.DiscountPercent = null;
            return discount;
        }


        public static Domain.Entities.Discount Create(byte discountPercent, int? priority = null, DateTime? startDate = null, DateTime? endDate = null,
            int? usedCount = null, int? maxUseCount = null)
        {
            Domain.Entities.Discount discount = Create(priority, startDate, endDate,usedCount , maxUseCount);
            discount.DiscountPercent = (int)discountPercent;
            return discount;
        }


        public static Product_Discount CreateProduct_Discount(Domain.Entities.Discount discount) 
        {
            Product_Discount product_Discount = new Product_Discount
            {
                Id = Guid.NewGuid(),
                Discount = discount,
                DiscountId = discount.Id,
            };
            return product_Discount;
        }







    }
}
