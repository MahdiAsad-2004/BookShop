using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Product : Entity<Guid>
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public int DiscountedPrice { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImageName { get; set; }
        public int? NumberOfInventory { get; set; }
        public int SellCount { get; set; }
        public ProductType ProductType { get; set; }
       


        public Book? Book { get; set; }
        public EBook? EBook { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Favorite> Favorites { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

    }
}
