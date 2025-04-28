using BookShop.Domain.Entities;
using BookShop.WebApi.Models.Author;
using BookShop.WebApi.Models.Publisher;
using BookShop.WebApi.Models.Review;
using BookShop.WebApi.Models.Translator;

namespace BookShop.WebApi.Models.Book
{
    public record BookDetailModel
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public float? DiscountedPrice { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImagePath { get; set; }
        public int NumberOfInventory { get; set; }
        public int NumberOfPages { get; set; }
        public Cover Cover { get; set; }
        public Cutting Cutting { get; set; }
        public Language Language { get; set; }
        public string? Shabak { get; set; }
        public DateTime PublishYear { get; set; }
        public float? WeightInGram { get; set; }
        public float ReviewsAcceptedAverageScore { get; set; }


        public PublisherModel Publisher { get; set; }
        public TranslatorModel? Translator { get; set; }
        public List<ReviewModel> Reviews { get; set; }
        public List<AuthorModel> Authors { get; set; }

    }
}
