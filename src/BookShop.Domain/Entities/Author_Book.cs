using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Author_Book : Entity<Guid>
    {
        public Guid AuthorId { get; set; }
        public Guid BookId { get; set; }


        public Author Author { get; set; }
        public Book Book { get; set; }

    }
}
