using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Author_EBook : Entity<Guid>
    {
        public Guid AuthorId { get; set; }
        public Guid EBookId { get; set; }


        public Author Author { get; set; }
        public EBook EBook { get; set; }

    }
}
