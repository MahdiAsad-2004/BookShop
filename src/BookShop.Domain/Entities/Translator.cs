using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Translator : Entity<Guid>
    {
        public int Name { get; set; }
        public string ImageName { get; set; }


        public IList<Book> Books { get; set; }
        public IList<EBook> EBooks { get; set; }

    }
}
