using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class User_Permission : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid PermissionId { get; set; }

        public User User { get; set; }
        public Permission Permission { get; set; }

    }
}
