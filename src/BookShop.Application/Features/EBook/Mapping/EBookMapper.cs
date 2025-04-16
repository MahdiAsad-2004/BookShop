using BookShop.Application.Features.Book.Commands.Update;
using BookShop.Application.Features.EBook.Commands.Create;
using BookShop.Application.Features.EBook.Commands.Update;
using BookShop.Domain.Enums;

namespace BookShop.Application.Features.EBook.Mapping
{
    public static class EBookMapper
    {
        public static void ToEBookAndProduct(CreateEBookCommand command, out E.EBook book, out E.Product product)
        {
            book = new E.EBook
            {
                Edition = command.Edition,
                Language = command.Language,
                NumberOfPages = command.NumberOfPages,
                PublisherId = command.PublisherId,
                PublishYear = command.PublishYear,
            };
            product = new E.Product
            {
                CategoryId = command.Product_CategoryId,
                DescriptionHtml = command.Product_DescriptionHtml,
                ImageName = string.Empty,
                NumberOfInventory = command.Product_NumberOfInventory,
                Price = command.Product_Price,
                ProductType = ProductType.EBook,
                SellCount = 0,
                Title = command.Product_Title,
            };
        }




        public static E.EBook ToEBookAndProduct(UpdateEBookCommand command, E.EBook book)
        {
            book.Product.CategoryId = command.Product_CategoryId;
            book.Product.DescriptionHtml = command.Product_DescriptionHtml;
            book.Product.NumberOfInventory = command.Product_NumberOfInventory;
            book.Product.Price = command.Product_Price;
            book.Product.Title = command.Product_Title;
            book.Edition = command.Edition;
            book.Language = command.Language;
            book.NumberOfPages = command.NumberOfPages;
            book.PublisherId = command.PublisherId;
            book.PublishYear = command.PublishYear;
            book.TranslatorId = command.TranslatorId;
            return book;
        }











    }

}
