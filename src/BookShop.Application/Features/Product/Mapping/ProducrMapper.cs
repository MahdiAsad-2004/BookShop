
using AutoMapper;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Domain.Entities;

namespace BookShop.Application.Features.Product.Mapping
{
    public class ProducrMapper : Profile
    {
        public ProducrMapper()
        {
            CreateMap<Domain.Entities.Product, ProductSummaryDto>()
                .ForMember(m => m.DiscountPercentage, a => a.MapFrom(b => b.DiscountedPrice > 0 ? (b.Price - b.DiscountedPrice) * 100f / b.Price : null));
                //.ForMember(m => m.rev, a => a.MapFrom(b => b.DiscountedPrice > 0 ? (b.Price - b.DiscountedPrice) * 100f / b.Price : null));
                //.ForMember(m => m.ReviewsAcceptedAverageScore , a => 
                   // a.MapFrom(b => b.Reviews != null && b.Reviews.Any(a => a.IsAccepted) ? (float)b.Reviews.Where(a => a.IsAccepted).Average(r => r.Score) : 0f ));


        }

    }
}
