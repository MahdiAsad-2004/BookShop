using BookShop.Domain.Entities;

namespace BookShop.Infrstructure.Persistance.QueryOptions
{
    internal class EBookQueryResultObject
    {
        public EBook EBook { get; set; }
        public Product? Product { get; set; }
        public Discount? MostPriorityValidDiscount { get; set; }
        public List<Review>? Reviews { get; set; }
        public Publisher? Publisher { get; set; }
        public Translator? Translator { get; set; }
        public Author_EBook[]? Author_EBooks { get; set; }


        public EBook MapToBook()
        {
            EBook Ebook = EBook;
            Ebook.Product = new Product(Product ,MostPriorityValidDiscount?.CalculateDiscountedPrice(Product.Price));
            Ebook.Publisher = Publisher;
            Ebook.Translator = Translator;
            Ebook.Author_EBooks = Author_EBooks;
            Ebook.Product.Reviews = Reviews;
            
            return Ebook;
        }

    }
}
