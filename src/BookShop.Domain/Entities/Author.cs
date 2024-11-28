using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Author : Entity<Guid>
    {
        public string Name { get; set; }
        public string ImageName { get; set; }


        public IList<Book> Books { get; set; }
        public IList<EBook> EBooks { get; set; }

    }
}
