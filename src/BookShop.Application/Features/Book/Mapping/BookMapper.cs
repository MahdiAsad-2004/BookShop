
using AutoMapper;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using MediatR;

namespace BookShop.Application.Features.Book.Mapping
{
    public class BookMapperProfile : Profile
    {
        public BookMapperProfile()
        {
            CreateMap<Domain.Entities.Book, BookDetailDto>()
                .ForMember(m => m.Authors, a => a.MapFrom(b => b.Author_Books.Select(a => a.Author).ToList()))
                .ForMember(m => m.DescriptionHtml, a => a.MapFrom(b => b.Product.DescriptionHtml))
                .ForMember(m => m.DiscountedPrice, a => a.MapFrom(b => b.Product.DiscountedPrice))
                .ForMember(m => m.ImageName, a => a.MapFrom(b => b.Product.ImageName))
                .ForMember(m => m.NumberOfInventory, a => a.MapFrom(b => b.Product.NumberOfInventory))
                .ForMember(m => m.Price, a => a.MapFrom(b => b.Product.Price))
                .ForMember(m => m.ReviewsAccepted, a => a.MapFrom(b => b.Product.Reviews.Where(a => a.IsAccepted)))
                .ForMember(m => m.ReviewsAcceptedAverageScore, a => a.MapFrom(b => 
                    b.Product.Reviews != null && b.Product.Reviews.Any(r => r.IsAccepted) ? (float)b.Product.Reviews.Where(s => s.IsAccepted).Average(s => s.Score) : 0f))
                .ForMember(m => m.Title, a => a.MapFrom(b => b.Product.Title));
                
            
            CreateMap<Domain.Entities.Book , BookSummaryDto>()
                .ForMember(m => m.DiscountPercentage, a => a.MapFrom(b => b.Product.DiscountedPrice > 0 ? 
                    (b.Product.Price - b.Product.DiscountedPrice) * 100f / b.Product.Price : null))
                .ForMember(m => m.ProductId , a => a.MapFrom(b => b.ProductId));
        }
    }



    public static class BookMapper
    {
        public static void ToBookAndProduct(CreateBookCommand command ,out Domain.Entities.Book book , out Domain.Entities.Product product)
        {
            book = new Domain.Entities.Book
            {
                Cover = command.Cover,
                Cutting = command.Cutting,
                Edition = command.Edition,
                Language = command.Language,
                NumberOfPages = command.NumberOfPages,
                PublisherId = command.PublisherId,
                PublishYear = command.PublishYear,
                Shabak = Guid.NewGuid().ToString().Substring(0, 12),
                WeightInGram = command.WeightInGram,
            };
            product = new Domain.Entities.Product
            {
                CategoryId = command.Product_CategoryId,
                DescriptionHtml = command.Product_DescriptionHtml,
                ImageName = string.Empty,
                NumberOfInventory = command.Product_NumberOfInventory,
                Price = command.Product_Price,
                ProductType = ProductType.Book,
                SellCount = 0,
                Title = command.Product_Title,
            };
        }


    }








}
