using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Domain.QueryOptions
{
    public class ProductQueryOption : IQueryOption<Product,Guid>
    {
        public bool IncludeReviews = false;

        public bool IncludeDiscounts = false;

        public int? StartPrice = null;
        
        public int? EndPrice = null;
        public ProductType? ProductType { get; set; }
        public bool? Available { get; set; }
        public byte? AverageScore { get; set; }

    }

    public enum ProductSortingOrder
    {
        Newest , Oldest,
        HighestPrice , LowestPrice,
        HighestDiscount , LowestDiscount,
        HighestSellCount , LowestSellCount,
        AlphabetDesc , AlphabetAsce
    }

}
