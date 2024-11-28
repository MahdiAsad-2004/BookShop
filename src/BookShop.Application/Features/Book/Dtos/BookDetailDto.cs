
using BookShop.Application.Common.Dtos;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Application.Features.Book.Dtos
{
    public class BookDetailDto : BaseDto
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public float? DiscountedPrice { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImageName { get; set; }
        public int NumberOfInventory { get; set; }
        public int NumberOfPages { get; set; }
        public Cover Cover { get; set; }
        public Cutting Cutting { get; set; }
        public Languages Language { get; set; }
        public string? Shabak { get; set; }
        public DateTime PublishYear { get; set; }
        public float? WeightInGram { get; set; }
        public float ReviewsAcceptedAverageScore { get; set; }


        public List<Review> ReviewsAccepted { get; set; }
        public Publisher Publisher { get; set; }
        public Translator? Translator { get; set; }
        public List<Author> Authors { get; set; }

    }
}
