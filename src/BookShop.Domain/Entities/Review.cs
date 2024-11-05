using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Review : Entity<Guid>
    {
        public byte Score { get; set; }
        public string Text { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Guid? UserId { get; set; }
        public Guid ProductId { get; set; }
        

        public User? User { get; set; }
        public Product Product { get; set; }
        
    }
}
