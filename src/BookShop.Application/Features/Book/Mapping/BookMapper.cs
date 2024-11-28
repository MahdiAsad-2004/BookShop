
using AutoMapper;
using BookShop.Application.Features.Book.Dtos;

namespace BookShop.Application.Features.Book.Mapping
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<Domain.Entities.Book, BookDetailDto>()
                .ForMember(m => m.Authors, a => a.MapFrom(b => b.Authors.ToList()))
                .ForMember(m => m.DescriptionHtml, a => a.MapFrom(b => b.Product.DescriptionHtml))
                .ForMember(m => m.DiscountedPrice, a => a.MapFrom(b => b.Product.DiscountedPrice))
                .ForMember(m => m.ImageName, a => a.MapFrom(b => b.Product.ImageName))
                .ForMember(m => m.NumberOfInventory, a => a.MapFrom(b => b.Product.NumberOfInventory))
                .ForMember(m => m.Price, a => a.MapFrom(b => b.Product.Price))
                .ForMember(m => m.ReviewsAccepted, a => a.MapFrom(b => b.Product.Reviews.Where(a => a.IsAccepted)))
                .ForMember(m => m.ReviewsAcceptedAverageScore, a => a.MapFrom(b => 
                    b.Product.Reviews != null && b.Product.Reviews.Any(r => r.IsAccepted) ? (float)b.Product.Reviews.Where(s => s.IsAccepted).Average(s => s.Score) : 0f))
                .ForMember(m => m.Title, a => a.MapFrom(b => b.Product.Title));
                


        }

    }
}
