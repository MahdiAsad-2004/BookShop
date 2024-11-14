using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Discount : Entity<Guid>
    {
        public string Name { get; set; }
        public int? DiscountPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public int Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public IEnumerable<Product_Discount> Product_Discounts { get; set; }




        #region methods

        public float CalculateDiscountedPrice(int price)
        {
            if (DiscountPrice != null)
                return price - DiscountPrice.Value;

            else if(DiscountPercent != null)
            {
                float floatPrice = (float)price;
                return floatPrice - (floatPrice * DiscountPercent.Value);
            }

            return price;
        }

        #endregion

    }
}
