
using BookShop.Application.Features.Product.Dtos;

namespace BookShop.Application.Features.Book.Dtos
{
    public class BookSummaryDto : ProductSummaryDto
    {
        public string ProductId { get; set; }

    }
}
