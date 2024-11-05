using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Publisher : Entity<Guid>
    {
        public int Title { get; set; }
        public string ImageName { get; set; }


        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<EBook> EBooks { get; set; }


    }
}
