using BookShop.Domain.Common.Entity;
using BookShop.Domain.Enums;

namespace BookShop.Domain.Entities
{
    public class Translator : Entity<Guid>
    {
        public string Name { get; set; }
        public string? ImageName { get; set; }
        public Gender Gender { get; set; }


        public IList<Book> Books { get; set; }
        public IList<EBook> EBooks { get; set; }

    }
}
