using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Translator : Entity<Guid>
    {
        public int Name { get; set; }
        public string ImageName { get; set; }


        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<EBook> EBooks { get; set; }

    }
}
