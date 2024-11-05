
using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class UserToken : Entity<Guid>
    {
        public string LoginProvider { get; set; }
        public string TokenName { get; set; }
        public string TokenValue { get; set; }
        public Guid UserId { get; set; }



        public User User { get; set; }

    }
}
