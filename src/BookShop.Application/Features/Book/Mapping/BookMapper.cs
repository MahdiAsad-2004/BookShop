using AutoMapper;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Application.Features.Book.Commands.Update;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Domain.Enums;

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
        public static void ToBookAndProduct(CreateBookCommand command ,out E.Book book , out E.Product product)
        {
            book = new E.Book
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
            product = new E.Product
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




        //public static void ToBookAndProduct(UpdateBookCommand command, ref E.Book book, ref E.Product product)
        //{
        //    product.CategoryId = command.Product_CategoryId;
        //    product.DescriptionHtml = command.Product_DescriptionHtml;
        //    product.NumberOfInventory = command.Product_NumberOfInventory;
        //    product.Price = command.Product_Price;
        //    product.Title = command.Product_Title;
        //    book.Cover = command.Cover;
        //    book.Cutting = command.Cutting;
        //    book.Edition = command.Edition;
        //    book.Language = command.Language;
        //    book.NumberOfPages = command.NumberOfPages;
        //    book.PublisherId = command.PublisherId;
        //    book.PublishYear = command.PublishYear;
        //    book.WeightInGram = book.WeightInGram;
        //}


        
        public static E.Book ToBookAndProduct(UpdateBookCommand command, E.Book book)
        {
            book.Product.CategoryId = command.Product_CategoryId;
            book.Product.DescriptionHtml = command.Product_DescriptionHtml;
            book.Product.NumberOfInventory = command.Product_NumberOfInventory;
            book.Product.Price = command.Product_Price;
            book.Product.Title = command.Product_Title;
            book.Cover = command.Cover;
            book.Cutting = command.Cutting;
            book.Edition = command.Edition;
            book.Language = command.Language;
            book.NumberOfPages = command.NumberOfPages;
            book.PublisherId = command.PublisherId;
            book.PublishYear = command.PublishYear;
            book.WeightInGram = command.WeightInGram;
            book.TranslatorId = command.TranslatorId;
            return book;
        }


        //public static BookDetailDto ToDto(E.Book book)
        //{
        //    var authors = book.Author_Books.Select(a => new E.Author
        //    {
        //        Author_Books = null,
        //        Author_EBooks = null,
        //        CreateBy = a.Author.CreateBy,
        //        CreateDate = a.Author.CreateDate,
        //        DeleteDate = a.Author.DeleteDate,
        //        DeletedBy = a.Author.DeletedBy,
        //        Gender = a.Author.Gender,
        //        Id = a.Author.Id,
        //        ImageName = a.Author.ImageName,
        //        IsDeleted = a.Author.IsDeleted,
        //        LastModifiedBy = a.Author.LastModifiedBy,
        //        LastModifiedDate = a.Author.LastModifiedDate,
        //        Name = a.
        //    });
        //    return new BookDetailDto
        //    {
        //        Id = book.Id,
        //        Authors 
        //    }
        //}








    }

}
