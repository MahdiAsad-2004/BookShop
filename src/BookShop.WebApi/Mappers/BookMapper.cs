using BookShop.Application.Extensions;
using BookShop.Application.Features.Book.Dtos;
using BookShop.WebApi.Models.Book;

namespace BookShop.WebApi.Mappers
{
    public static  class BookMapper
    {
        public static BookDetailModel ToDetailModel(BookDetailDto bookDetailDto)
        {
            return new BookDetailModel
            {
                Authors = AuthorMapper.ToModel(bookDetailDto.Authors),
                Cover = bookDetailDto.Cover,
                Cutting = bookDetailDto.Cutting,
                DescriptionHtml = bookDetailDto.DescriptionHtml,
                DiscountedPrice = bookDetailDto.DiscountedPrice,
                ImagePath = Path.Combine(PathExtensions.Product.Images , bookDetailDto.ImageName),
                Language = bookDetailDto.Language,
                NumberOfInventory = bookDetailDto.NumberOfInventory,
                NumberOfPages = bookDetailDto.NumberOfPages,
                Price = bookDetailDto.Price,    
                Publisher = PublisherMapper.ToModel(bookDetailDto.Publisher),
                PublishYear = bookDetailDto.PublishYear,
                Reviews = ReviewMapper.ToModel(bookDetailDto.ReviewsAccepted),
                ReviewsAcceptedAverageScore = bookDetailDto.ReviewsAcceptedAverageScore,
                Shabak = bookDetailDto.Shabak,
                Title = bookDetailDto.Title,
                Translator = bookDetailDto.Translator != null ? TranslatorMapper.ToModel(bookDetailDto.Translator) : null,
                WeightInGram = bookDetailDto.WeightInGram,
            };
        }

    }
}
