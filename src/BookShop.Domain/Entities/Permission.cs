
using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class Permission : Entity<Guid>
    {
        public string Name { get; set; }



        public IList<User_Permission> User_Permissions { get; set; }

    }
}
