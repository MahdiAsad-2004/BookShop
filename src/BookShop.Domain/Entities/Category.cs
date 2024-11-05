using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Category : Entity<Guid>
    {
        public string Title { get; set; }
        public string ImageName { get; set; }
        public Guid? ParentId { get; set; }


        public Category? Parent { get; set; }
        public IEnumerable<Category> Childs { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }
}
