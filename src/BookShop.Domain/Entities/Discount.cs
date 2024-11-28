using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Discount : Entity<Guid>
    {
        public string Name { get; set; }
        public int? DiscountPrice { get; set; }
        public int UsedCount { get; set; }
        public int? MaximumUseCount { get; set; }
        public float? DiscountPercent { get; set; }
        public int Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public IList<Product_Discount> Product_Discounts { get; set; }




        #region methods

        public float? CalculateDiscountedPrice(int price)
        {
            if (DiscountPrice != null)
                return price - DiscountPrice.Value;

            else if(DiscountPercent != null)
            {
                float floatPrice = (float)price;
                return floatPrice - (floatPrice * DiscountPercent.Value / 100f);
            }

            return null;
        }

        public bool IsValid()
        {
            if (StartDate != null && DateTime.UtcNow < StartDate)
                return false;

            if (EndDate != null && DateTime.UtcNow > EndDate)
                return false;

            if(DiscountPercent == null && DiscountPrice == null) 
                return false;
            
            if(MaximumUseCount != null && UsedCount >= MaximumUseCount) 
                return false;

            return true;
        }

        public static float? CalculateDiscountedPrice(int price , int? discountedPrice , float? discountedPercent)
        {
            if (discountedPrice != null)
                return price - discountedPrice.Value;

            else if (discountedPercent != null)
            {
                float floatPrice = (float)price;
                return floatPrice - (floatPrice * discountedPercent.Value / 100f);
            }

            return null;
        }



        #endregion

    }
}
