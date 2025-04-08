using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Author : Entity<Guid>
    {
        public string Name { get; set; }
        public string? ImageName { get; set; }
        public Gender Gender { get; set; }


        public IList<Author_EBook> Author_EBooks { get; set; }
        public IList<Author_Book> Author_Books { get; set; }

    }
}
