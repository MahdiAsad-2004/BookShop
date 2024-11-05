using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class RoleClaim : Entity<Guid>
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public Guid RoleId { get; set; }



        public Role Role { get; set; }



    }
}
