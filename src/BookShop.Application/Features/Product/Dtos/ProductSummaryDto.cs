
using BookShop.Application.Common.Dtos;

namespace BookShop.Application.Features.Product.Dtos
{
    public class ProductSummaryDto : BaseDto
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }
        public int NumberOfInventory { get; set; }
        public float ReviewsAverageScore { get; set; }
        public float? DiscountedPrice { get; set; }
        public byte? DiscountPercentage { get; set; }


    }
}
