using BookShop.Domain.Entities;

namespace BookShop.Infrstructure.Persistance.QueryOptions
{
    internal class BookQueryResultObject
    {
        public Book Book { get; set; }
        public Product Product { get; set; }
        public Discount? MostPriorityValidDiscount { get; set; }
        public List<Review>? Reviews { get; set; }
        public Publisher? Publisher { get; set; }
        public Translator? Translator { get; set; }
        public Author_Book[]? Author_Books { get; set; }


        public Book MapToBook()
        {
            Book book = Book;
            book.Product = new Product(Product ,MostPriorityValidDiscount?.CalculateDiscountedPrice(Product.Price));
            book.Publisher = Publisher;
            book.Translator = Translator;
            book.Author_Books = Author_Books;
            book.Product.Reviews = Reviews;
            //book.Product.DiscountedPrice = MostPriorityValidDiscount?.CalculateDiscountedPrice(Product.Price);

            return book;
        }

    }
}
