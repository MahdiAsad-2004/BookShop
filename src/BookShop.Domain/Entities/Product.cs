using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Product : Entity<Guid>
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public float? DiscountedPrice { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImageName { get; set; }
        public int NumberOfInventory { get; set; }
        public int SellCount { get; set; }
        public ProductType ProductType { get; set; }
        public float? ReviewsAcceptedAverageScore { get; set; }



        public Book? Book { get; set; }
        public EBook? EBook { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<Favorite> Favorites { get; set; }
        public IList<Review> Reviews { get; set; }
        public IList<Product_Discount> Product_Discounts { get; set; }




        public float FinalPrice()
        {
            float? discountedPrice = GetDscountedPrice();
            if (discountedPrice > 0)
                return discountedPrice.Value;
                
            return Price;
        }


        public float? GetDscountedPrice()
        {
            if (Product_Discounts != null && Product_Discounts.Any())
            {
                var discounts = Product_Discounts.Select(a => a.Discount);
                if (discounts != null && discounts.Any(a => a.IsValid()))
                {
                    var discount = discounts.OrderBy(a => a.Priority).First(a => true);
                    return discount.CalculateDiscountedPrice(Price);
                }
            }
            return null;
        }


        public float GetReviewsAcceptedAverageScore()
        {
            if (Reviews != null && Reviews.Any(a => a.IsAccepted))
            {
                return (float)Reviews.Where(a => a.IsAccepted).Average(a => a.Score);
            }
            return 0f;
        }

      

        public float? GetDscountedPrice(Discount? discount)
        {
            if (discount != null)
            {
                if (discount.IsValid())
                {
                    return discount.CalculateDiscountedPrice(Price);
                }
                return 0f;
            }
            return null;
        }

    }


}
