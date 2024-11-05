using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class User : Entity<Guid> 
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUsername { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string? SecurityStamp { get; set; }
        public string ImageName { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }




        public IEnumerable<UserClaim> UserClaims { get; set; }
        public IEnumerable<User_Role> User_Roles { get; set; }
        public IEnumerable<AuditLog> AuditLogs { get; set; }
        public IEnumerable<Favorite> Favorites { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<PasswordHistory> PasswordHistories { get; set; }
        public IEnumerable<UserToken> UserTokens { get; set; }
        public IEnumerable<User_Permission> User_Permissions { get; set; }

    }
}
