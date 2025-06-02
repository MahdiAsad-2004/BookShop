using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Product : Entity<Guid>
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public string DescriptionHtml { get; set; }
        public string? ImageName { get; set; }
        public int NumberOfInventory { get; set; }
        public int SellCount { get; set; }
        public ProductType ProductType { get; set; }
        public Guid? CategoryId { get; set; }


        private float? _ReviewsAcceptedAverageScore;

        private float? _DiscountedPrice;


        #region computed columns

        // Computed Column
        public float? DiscountedPrice
        {
            get
            {
                if (_DiscountedPrice == null)
                    _DiscountedPrice = GetDiscountedPrice();
                return _DiscountedPrice;

                //if (_DiscountedPrice == null)
                //    return GetDiscountedPrice();
                //return _DiscountedPrice;
            }
            init { _DiscountedPrice = value; }
        }

        // Computed Column
        public float? ReviewsAcceptedAverageScore
        {
            get
            {
                if (_ReviewsAcceptedAverageScore == null)
                    return GetReviewsAcceptedAverageScore();
                return _ReviewsAcceptedAverageScore;
            }
            init { _ReviewsAcceptedAverageScore = value; }
        }

        #endregion


        public Book? Book { get; set; }
        public EBook? EBook { get; set; }
        public Category? Category { get; set; }
        public IList<CartItem> CartItems { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public IList<Favorite> Favorites { get; set; }
        public IList<Review> Reviews { get; set; }
        //public IList<Product_Category> Product_Categories { get; set; }
        public IList<Product_Discount> Product_Discounts { get; set; }









        public Product() {}
        public Product(Product product , float? discountedPrice)
        {
            this.Book = product.Book;
            this.Category = product.Category;
            this.CategoryId = product.CategoryId;
            this.CreateBy = product.CreateBy;
            this.CreateDate = product.CreateDate;
            this.DeleteDate = product.DeleteDate;
            this.DescriptionHtml = product.DescriptionHtml;
            this.DiscountedPrice = discountedPrice;
            this.EBook = product.EBook;
            this.Favorites = product.Favorites;
            this.Id = product.Id;
            this.ImageName = product.ImageName;
            this.IsDeleted = product.IsDeleted;
            this.LastModifiedBy = product.LastModifiedBy;
            this.LastModifiedDate = product.LastModifiedDate;
            this.NumberOfInventory = product.NumberOfInventory;
            this.Price = product.Price;
            this.ProductType = product.ProductType;
            this.Product_Discounts = product.Product_Discounts;
            this.Reviews = product.Reviews;
            this.ReviewsAcceptedAverageScore = product.ReviewsAcceptedAverageScore;
            this.SellCount = product.SellCount;
            this.Title = product.Title;


        }


        public float FinalPrice()
        {
            float? discountedPrice = DiscountedPrice;
            if (discountedPrice > 0)
                return discountedPrice.Value;
                
            return Price;
        }

        private float? GetDiscountedPrice()
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

        private float GetReviewsAcceptedAverageScore()
        {
            if (Reviews != null && Reviews.Any(a => a.IsAccepted))
            {
                return (float)Reviews.Where(a => a.IsAccepted).Average(a => a.Score);
            }
            return 0f;
        }

        public float? GetDiscountedPrice(Discount? discount)
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
