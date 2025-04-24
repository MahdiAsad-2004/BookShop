
using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class RefreshToken : Entity<Guid>
    {
        public string TokenValue { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool Revoked { get; set; }


        public User User { get; set; }

    }
}
