using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Category : Entity<Guid>
    {
        public string Title { get; set; }
        public string ImageName { get; set; }
        public Guid? ParentId { get; set; }


        public Category? Parent { get; set; }
        public IList<Category> Childs { get; set; }
        public IList<Product_Category> Product_Categories { get; set; }

    }

}
