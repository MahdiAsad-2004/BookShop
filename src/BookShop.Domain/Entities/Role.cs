using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Role : Entity<Guid>
    {
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string? NormalizedName { get; set; }


        public IList<RoleClaim> RoleClaims { get; set; }
        public IList<User_Role> User_Roles { get; set; }

    }
}
